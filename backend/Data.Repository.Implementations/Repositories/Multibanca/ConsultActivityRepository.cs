using Data.Extensions.Repository;
using Data.Repository.Interfaces.Repositories.Multibanca;
using Microsoft.EntityFrameworkCore;
using Multibanca.DTO.Common;
using Multibanca.DTO.Multibanca;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.Implementations.Repositories.Multibanca
{
    public class ConsultActivityRepository : IConsultActivityRepository
    {
        private readonly MultibancaDBContext MultibancaDBContext;

        public ConsultActivityRepository(MultibancaDBContext _multibancaDBContext)
        {
            MultibancaDBContext = _multibancaDBContext;
        }

        public async Task<List<ControlBaseDTO>> GetCatalogoTipoBusqueda()
        {
            List<ControlBaseDTO> listControlBaseDTO = new List<ControlBaseDTO>();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                SELECT *
                FROM usp_get_catalogo_tipo_busqueda_solicitud();
            ";

                    command.CommandType = CommandType.Text;

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    listControlBaseDTO = reader.MapToListDomain<ControlBaseDTO>();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return listControlBaseDTO;
            }
        }

        public async Task<List<ConsultActivityDTO>> GetConsultActivity(SearchCriteriaDTO searchCriteria)
        {
            List<ConsultActivityDTO> responseList = new List<ConsultActivityDTO>();

            object data = new
            {
                p_option = searchCriteria.option,
                p_search_criteria = searchCriteria.search_criteria
            };

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                SELECT *
                FROM usp_select_bandeja_consultas(
                    @p_option,
                    @p_search_criteria
                );
            ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    responseList = reader.MapToListDomain<ConsultActivityDTO>();
                }
                catch (Exception ex)
                {
                    responseList = null;
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return responseList;
            }
        }

        public async Task<List<EtapaDTO>> GetConsultTrackinActivity(long idExpediente)
        {
            object data = new
            {
                p_id_expediente = idExpediente
            };

            List<EtapaDTO> listEtapas = new List<EtapaDTO>();
            List<ActividadDTO> listActividades = new List<ActividadDTO>();

            using (DbCommand command = MultibancaDBContext.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    await MultibancaDBContext.Database.OpenConnectionAsync();

                    command.CommandText = @"
                SELECT *
                FROM usp_consult_trackin_activity_etapa(
                    @p_id_expediente
                );

                SELECT *
                FROM usp_consult_trackin_activity_actividades(
                    @p_id_expediente
                );
            ";

                    command.CommandType = CommandType.Text;
                    command.Parameters.ToArray<object>(data);

                    DbDataReader reader = await command.ExecuteReaderAsync();

                    listEtapas = reader.MapToListDomain<EtapaDTO>();

                    await reader.NextResultAsync();

                    listActividades = reader.MapToListDomain<ActividadDTO>();

                    foreach (EtapaDTO etapa in listEtapas)
                    {
                        etapa.actividades = new List<ActividadDTO>();
                        etapa.title = !string.IsNullOrWhiteSpace(etapa.etapa)
                            ? etapa.etapa.Split(' ')
                            : Array.Empty<string>();

                        foreach (ActividadDTO actividad in listActividades)
                        {
                            if (etapa.id_etapa == actividad.id_etapa)
                            {
                                etapa.actividades.Add(actividad);
                            }
                        }
                    }

                    foreach (EtapaDTO etapa in listEtapas)
                    {
                        int countComplete = 0;
                        int countInProgress = 0;
                        int countNotStarted = 0;

                        foreach (ActividadDTO actividad in etapa.actividades)
                        {
                            actividad.title = !string.IsNullOrWhiteSpace(actividad.actividad)
                                ? actividad.actividad.Split(' ')
                                : Array.Empty<string>();

                            switch (actividad.estado)
                            {
                                case "Completada":
                                    actividad.estado = "Done";
                                    countComplete++;
                                    break;

                                case "En progreso":
                                case "En Progreso":
                                    actividad.estado = "InProgress";
                                    countInProgress++;
                                    break;

                                case "Nueva":
                                    actividad.estado = "NotStarted";
                                    countNotStarted++;
                                    break;

                                default:
                                    actividad.estado = "NotStarted";
                                    countNotStarted++;
                                    break;
                            }
                        }

                        if (etapa.actividades.Count == 0)
                        {
                            etapa.estado = "NotStarted";
                        }
                        else if (countComplete == etapa.actividades.Count)
                        {
                            etapa.estado = "Done";
                        }
                        else if (countInProgress > 0)
                        {
                            etapa.estado = "InProgress";
                        }
                        else if (countComplete > 0)
                        {
                            etapa.estado = "Done";
                        }
                        else
                        {
                            etapa.estado = "NotStarted";
                        }

                        etapa.current_actividades = countComplete;
                    }

                    foreach (EtapaDTO etapa in listEtapas)
                    {
                        for (int i = etapa.actividades.Count - 1; i >= 0; i--)
                        {
                            if (string.IsNullOrWhiteSpace(etapa.actividades[i].estado))
                            {
                                etapa.actividades.RemoveAt(i);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    listActividades = null;
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                    await MultibancaDBContext.Database.CloseConnectionAsync();
                }

                return listEtapas;
            }
        }
    }
}
