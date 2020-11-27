using System.Collections.Generic;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.BusinessEntityServices.Data
{
    public static class StateDetailMappers
    {
        public static StateDetailDto ToStateDetailDto(this StateDetail stateDetail, string businessEntityId) => new StateDetailDto
        {
            Id = stateDetail.Id,
            BusinessEntityId = businessEntityId,
            State = stateDetail.State,
            TimeStampUtc = stateDetail.TimeStampUtc
        };

        public static StateDetail ToStateDetail(this StateDetailDto stateDetailDto) => new StateDetail
        {
            Id = stateDetailDto.Id,
            State = stateDetailDto.State,
            TimeStampUtc = stateDetailDto.TimeStampUtc
        };
    }
}
