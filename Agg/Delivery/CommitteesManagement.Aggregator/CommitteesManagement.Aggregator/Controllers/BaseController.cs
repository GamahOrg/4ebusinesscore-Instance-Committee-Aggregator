namespace CommitteesManagement.Aggregator.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize("ResourceOwnerPolicy")]
	public class BaseController:ControllerBase
	{
	}
}
