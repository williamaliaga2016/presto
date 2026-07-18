using AutoMapper;
using Data.Repository.Interfaces.Entities.FuncTransversal;
using Multibanca.Domain.Models.FuncTransversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multibanca.Automapper.FuncTransversal
{
    public class AutoMapperProfileFuncTransversal : Profile
    {
        public AutoMapperProfileFuncTransversal()
        {
            CreateMap<bitacora, bitacora_entity>().ReverseMap();
            CreateMap<expediente_digital, expediente_digital_entity>().ReverseMap();
            CreateMap<expediente_digital, expediente_digital_mongo_entity>().ReverseMap();
        }
    }
}
