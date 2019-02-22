using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpWars.Mapping.Interfaces
{
    public interface IMapper<TModel, TDto>
    {
        TModel Map(TDto dto);

        IEnumerable<TModel> Map(IEnumerable<TDto> dtos);

        TDto Map(TModel model);

        IEnumerable<TDto> Map(IEnumerable<TModel> models);
    }
}