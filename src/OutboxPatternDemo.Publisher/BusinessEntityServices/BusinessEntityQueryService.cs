using System.Linq;
using OutboxPatternDemo.Publisher.BusinessEntityServices.Data;
using OutboxPatternDemo.Publisher.Contract.Models;

namespace OutboxPatternDemo.Publisher.BusinessEntityServices;

public interface IBusinessEntityQueryService
{
    public BusinessEntity GetBusinessEntityById(string id);
}

public class BusinessEntityQueryService : IBusinessEntityQueryService
{
    private readonly BusinessEntityContext _stateDetailContext;

    public BusinessEntityQueryService(BusinessEntityContext stateDetailContext)
    {
        _stateDetailContext = stateDetailContext;
    }

    public BusinessEntity GetBusinessEntityById(string id)
    {
        var stateDetails = _stateDetailContext.StateDetails
            .Where(sd => sd.BusinessEntityId == id)
            .Select(sd => sd.ToStateDetail());

        return new BusinessEntity(id, stateDetails.ToList());
    }
}
