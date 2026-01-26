namespace CommitteesManagement.Aggregator.Controllers
{
	public class ProceedingController:BaseController
	{
		private readonly IProceedingService _proceedingService;

		public ProceedingController(IProceedingService proceedingService)
		{
			_proceedingService = proceedingService;
		}

		[HttpGet("{proceedingId}")]
		public async Task<ResponseDTO> GeProceedingById(Guid proceedingId)
		{
			return await _proceedingService.GeProceedingById(proceedingId);
		}

	}
}
