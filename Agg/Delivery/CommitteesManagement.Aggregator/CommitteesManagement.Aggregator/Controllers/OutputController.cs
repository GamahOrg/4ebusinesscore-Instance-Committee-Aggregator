namespace CommitteesManagement.Aggregator.Controllers
{
	public class OutputController:BaseController
	{
		private readonly IOutputService _outputService;

		public OutputController(IOutputService outputService)
		{
			_outputService = outputService;
		}

		[HttpGet("GetOutputAttachments/{outputId}")]
		public async Task<ResponseDTO> GetOutputAttachments(Guid outputId)
		{
			return await _outputService.GetOutputAttachments(outputId);
		}

        [HttpGet("GetOutputsByCommitteeId/{committeeId}")]
        public async Task<ResponseDTO> GetOutputsByCommitteeId(Guid committeeId)
        {
            return await _outputService.GetOutputsByCommitteeId(committeeId);
        }
    }
}
