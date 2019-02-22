using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CSharpWars.Mapping.Interfaces;

namespace CSharpWars.Mapping
{
    public abstract class Mapper<TModel, TDto> : IMapper<TModel, TDto>
    {
        public abstract TModel Map(TDto dto);

        public IEnumerable<TModel> Map(IEnumerable<TDto> dtos)
        {
            return dtos?.Select(Map);
        }

        public abstract TDto Map(TModel model);

        public IEnumerable<TDto> Map(IEnumerable<TModel> models)
        {
            return models?.Select(Map);
        }
    }
}