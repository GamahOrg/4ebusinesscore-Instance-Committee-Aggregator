namespace CommitteesManagement.Aggregator.Controllers
{
    public class MeetingController : BaseController
    {
        private readonly IMeetingService _meetingService;

        public MeetingController(IMeetingService meetingService)
        {
            _meetingService = meetingService;
        }

        [HttpGet("GetAllMeetingAttachments/{meetingId}")]
        public async Task<ResponseDTO> GetMeetingAttachments(Guid meetingId)
        {
            return await _meetingService.GetMeetingAttachments(meetingId);
        }

        [HttpGet("GetMeetingByCommitteeId/{committeeId}")]
        public async Task<ResponseDTO> GetMeetingByCommitteeId(Guid committeeId)
        {
            return await _meetingService.GetMeetingByCommitteeId(committeeId);
        }

        [HttpGet("GetMeetingById/{meetingId}")]
        public async Task<ResponseDTO> GetMeetingById(Guid meetingId)
        {
            return await _meetingService.GetMeetingById(meetingId);
        }

        [HttpGet("GetMeetingByName/{meetingName}")]
        public async Task<ResponseDTO> GetMeetingByName(string meetingName)
        {
            return await _meetingService.GetMeetingByName(meetingName);
        }
    }
}
