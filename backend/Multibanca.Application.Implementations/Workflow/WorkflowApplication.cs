using AutoMapper;
using Multibanca.Application.Interfaces.Workflow;
using Framework.WorkFlow.Common.DTO;
using Multibanca.Application.Interfaces.Common;
using Multibanca.Application.Interfaces.Multibanca;
using Multibanca.Common;
using Multibanca.Domain.Models.Multibanca;
using Framework.WorkFlow.Application.Interfaces;

namespace Multibanca.Application.Implementations.Workflow
{
    public class WorkflowApplication : IWorkflowApplication
    {
        private readonly ICaseApplication CaseApplicationProvider;
        private readonly IActividadesApplication ActivityApplicationProvider;
        private readonly ICommonApplication CommonApplicationProvider;
        private readonly IMapper Mapper;
        public WorkflowApplication(ICaseApplication _caseApplication, IActividadesApplication _activityApplication, ICommonApplication _commonApplication, IMapper _mapper)
        {
            CaseApplicationProvider = _caseApplication;
            ActivityApplicationProvider = _activityApplication;
            CommonApplicationProvider = _commonApplication;
            Mapper = _mapper;
        }

        public async Task<FolioDTO> CreateCase(string title, string workFlowId, string performerId, int userId)
        {
            FolioDTO folioDTO = new FolioDTO { id_actividad = String.Empty };

            try
            {
                business_case_DTO caseInstance = await CaseApplicationProvider.CreateCase(Constants.WorkFlowMultibanca.WorkFlowID, "Principal", "admin");

                if (caseInstance != null & caseInstance.case_id > 0)
                {
                    if (!(caseInstance.current_activities.Count() > 1))
                    {
                        AssignActivityDTO assignActivityDTO = await CommonApplicationProvider.AsignarActividad(caseInstance.case_id, performerId);

                        if (assignActivityDTO != null)
                        {
                            actividades actividad = new actividades();
                            actividad.id_expediente = caseInstance.case_id;
                            actividad.id_actividad = caseInstance.current_activities[0].activity_id;
                            actividad.id_rol = assignActivityDTO.id_rol;
                            actividad.id_usuario = userId;
                            actividad.descripcion = caseInstance.current_activities[0].display_name;
                            actividad.status = "Nueva";
                            actividad.activo = true;
                            actividad.fecha_asignacion = DateTime.Now;
                            actividad.fecha_alta = DateTime.Now;
                            await ActivityApplicationProvider.InsertActividad(actividad);
                            folioDTO.id_expediente = caseInstance.case_id;
                            folioDTO.id_actividad = actividad.id_actividad;
                        }
                        else
                        {
                            Exception ex = new Exception("No se encontro informacion del Usuario con el Id: " + assignActivityDTO.id_usuario.ToString());
                            throw (ex);
                        }
                    }
                }
                return folioDTO;
            }
            catch (Exception ex)
            {
                Exception e = new Exception("Ocurrio un error al intentar Iniciar el Flujo.", ex);
                //ExceptionManager.HandleException(e, 1, 5000, 1);
                throw (e);
            }
        }

        public async Task<bool> CancelCase(long caseId)
        {
            try
            {
                return await CaseApplicationProvider.CancelCase(caseId);
            }
            catch (Exception ex)
            {
                Exception e = new Exception("Ocurrio un error al intentar Iniciar el Flujo.", ex);
                //ExceptionManager.HandleException(e, 1, 5000, 1);
                throw (e);
            }
        }

        public async Task<List<AssignActivityDTO>> AvanzarActividad(string actionId, FolioDTO oFolio, int id_usuario)
        {
            List<AssignActivityDTO> listAssignActivityDTO = new List<AssignActivityDTO>();
            try
            {
                var actividad = await ActivityApplicationProvider.ObtenerInformacionActividadPorId(oFolio.id);
                // var actividad = actividades.ObtenerInformacionActividadPorId(Convert.ToInt64(75764));
                if (actividad != null)
                {
                    //trae del WF
                    var caseInstance = await CaseApplicationProvider.GetCase(oFolio.id_expediente);

                    //var caseInstance = BusinessCaseManager.GetCase(Convert.ToInt64(21859));

                    if (caseInstance != null)
                    {
                        if (caseInstance.current_activities.Count() > 0)
                        {
                            foreach (var wfActividad in caseInstance.current_activities)
                                if (wfActividad.activity_id == actividad.id_actividad)
                                {
                                    try
                                    {
                                        caseInstance = await CaseApplicationProvider.DispatchActivity(actividad.id_expediente, actividad.id_actividad, actionId);//idWorkFlow
                                    }
                                    catch (Exception exception)
                                    {
                                        throw new ApplicationException(exception.Message);
                                    }

                                    if (caseInstance != null)
                                    {
                                        // Cerrar actividad actual
                                        await ActivityApplicationProvider.CompletarActividad(actividad.id, id_usuario);

                                        if (caseInstance.current_activities.Count() > 0)
                                        {
                                            foreach (var newActividades in caseInstance.current_activities)
                                            {
                                                bool existeActividad = await ActivityApplicationProvider.ExisteActividadActiva(actividad.id_expediente, newActividades.activity_id);

                                                if (!existeActividad)
                                                {
                                                    try
                                                    {
                                                        AssignActivityDTO assignActivityDTO = new AssignActivityDTO();

                                                        assignActivityDTO = await CommonApplicationProvider.AsignarActividad(actividad.id_expediente, newActividades.performer);

                                                        actividades model = new actividades();

                                                        model.id_expediente = actividad.id_expediente;
                                                        model.id_actividad = newActividades.activity_id;
                                                        assignActivityDTO.id_actividad = newActividades.activity_id;
                                                        model.id_rol = assignActivityDTO.id_rol;
                                                        model.id_usuario = assignActivityDTO.id_usuario;
                                                        model.descripcion = newActividades.display_name;
                                                        assignActivityDTO.display_name = newActividades.display_name;
                                                        model.status = "Nueva";
                                                        model.activo = true;
                                                        model.fecha_asignacion = DateTime.Now;
                                                        model.fecha_alta = DateTime.Now;
                                                        long idNuevo = (ActivityApplicationProvider.Create(model, model.id_usuario)).id;

                                                        listAssignActivityDTO.Add(assignActivityDTO);
                                                    }
                                                    catch (Exception e)
                                                    {
                                                        throw new Exception("La actividad " + newActividades.display_name + "(" + newActividades.activity_id + ") no tiene asignado un Rol existente.", e);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var res = await CaseApplicationProvider.CloseCase(actividad.id_expediente);//idWorkFlow
                                        }

                                        return listAssignActivityDTO;
                                    }
                                    else
                                        throw new ApplicationException("No fue posible avanzar la actividad " + actividad.descripcion);
                                }

                            throw new ApplicationException("Usted no tiene acceso para procesar  Solicitud " + actividad.id_expediente.ToString() + " no esta relacionada a la actividad '" + actividad.descripcion + "'! Por favor verfique con el administrador del sistema");
                        }
                        else
                            throw new ApplicationException("La Solicitud " + actividad.id_expediente.ToString() + " no tiene actividades activas! Por favor verfique con el administrador del sistema");
                    }
                    else
                        throw new ApplicationException("El Id de la Solicitud es incorrecto! de la Solicitud " + actividad.id_expediente.ToString());
                }
                else
                    throw new ApplicationException("El Id de la Actividad es incorrecto! de la Solicitud " + actividad.id_expediente.ToString());

            }
            catch (Exception ex)
            {
                //Exception e = new Exception("Error al intentar Avanzar la Actividad para la Solicitud " + Solicitud.id_expediente.ToString(), ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<AssignActivityDTO> AvanzarActividad_UsuarioDefault(string actionId, FolioDTO oFolio, int id_usuario, int IdRolDefault, int IdUsuarioDefault)
        {
            AssignActivityDTO assignActivityDTO = new AssignActivityDTO();
            try
            {
                var actividad = await ActivityApplicationProvider.ObtenerInformacionActividadPorId(oFolio.id);
                // var actividad = actividades.ObtenerInformacionActividadPorId(Convert.ToInt64(75764));
                if (actividad != null)
                {
                    //trae del WF
                    var caseInstance = await CaseApplicationProvider.GetCase(oFolio.id_expediente);

                    //var caseInstance = BusinessCaseManager.GetCase(Convert.ToInt64(21859));

                    if (caseInstance != null)
                    {
                        if (caseInstance.current_activities.Count() > 0)
                        {
                            foreach (var wfActividad in caseInstance.current_activities)
                                if (wfActividad.activity_id == actividad.id_actividad)
                                {
                                    try
                                    {
                                        caseInstance = await CaseApplicationProvider.DispatchActivity(actividad.id_expediente, actividad.id_actividad, actionId);//idWorkFlow
                                    }
                                    catch (Exception exception)
                                    {
                                        throw new ApplicationException(exception.Message);
                                    }

                                    if (caseInstance != null)
                                    {
                                        // Cerrar actividad actual
                                        await ActivityApplicationProvider.CompletarActividad(actividad.id, id_usuario);

                                        if (caseInstance.current_activities.Count() > 0)
                                        {
                                            foreach (var newActividades in caseInstance.current_activities)
                                            {
                                                bool existeActividad = await ActivityApplicationProvider.ExisteActividadActiva(actividad.id_expediente, newActividades.activity_id);

                                                if (!existeActividad)
                                                {
                                                    try
                                                    {
                                                        //assignActivityDTO = await CommonApplicationProvider.AsignarActividad(actividad.id_expediente, newActividades.Performer);

                                                        actividades model = new actividades();

                                                        model.id_expediente = actividad.id_expediente;
                                                        //model.idWorkFlow = caseInstance.Id;
                                                        model.id_actividad = newActividades.activity_id;

                                                        //model.id_rol = assignActivityDTO.id_rol;
                                                        //model.id_usuario = assignActivityDTO.id_usuario;
                                                        model.id_rol = IdRolDefault;
                                                        model.id_usuario = IdUsuarioDefault;
                                                        assignActivityDTO.id_rol = IdRolDefault;
                                                        assignActivityDTO.id_usuario = IdUsuarioDefault;

                                                        model.descripcion = newActividades.display_name;
                                                        model.status = "Nueva";
                                                        model.activo = true;
                                                        model.fecha_asignacion = DateTime.Now;
                                                        model.fecha_alta = DateTime.Now;

                                                        long idNuevo = (ActivityApplicationProvider.Create(model,model.id_usuario)).id;


                                                    }
                                                    catch (Exception e)
                                                    {
                                                        throw new Exception("La actividad " + newActividades.display_name + "(" + newActividades.activity_id + ") no tiene asignado un Rol existente.", e);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            var res = await CaseApplicationProvider.CloseCase(actividad.id_expediente);//idWorkFlow
                                        }

                                        return assignActivityDTO;
                                    }
                                    else
                                        throw new ApplicationException("No fue posible avanzar la actividad " + actividad.descripcion);
                                }

                            throw new ApplicationException("Usted no tiene acceso para procesar  Solicitud " + actividad.id_expediente.ToString() + " no esta relacionada a la actividad '" + actividad.descripcion + "'! Por favor verfique con el administrador del sistema");
                        }
                        else
                            throw new ApplicationException("La Solicitud " + actividad.id_expediente.ToString() + " no tiene actividades activas! Por favor verfique con el administrador del sistema");
                    }
                    else
                        throw new ApplicationException("El Id de la Solicitud es incorrecto! de la Solicitud " + actividad.id_expediente.ToString());
                }
                else
                    throw new ApplicationException("El Id de la Actividad es incorrecto! de la Solicitud " + actividad.id_expediente.ToString());

            }
            catch (Exception ex)
            {
                //Exception e = new Exception("Error al intentar Avanzar la Actividad para la Solicitud " + Solicitud.id_expediente.ToString(), ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<FolioDTO> CapturarDatosFolio(long id_expediente, string ActivityID)
        {
            return await CommonApplicationProvider.CapturarDatosFolio(id_expediente, ActivityID);
        }

        public async Task<List<xpdl_transition_DTO>> GetTransitions(string ActivityId)
        {
            return await CaseApplicationProvider.GetTransitions(ActivityId);
        }

        public Task<bool> ExisteActividadFolio(long idExpediente, string idActividad)
        {
            return CommonApplicationProvider.ExisteActividadFolio(idExpediente, idActividad);
        }
    }
}
