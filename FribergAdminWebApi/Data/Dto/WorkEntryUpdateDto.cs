using System.ComponentModel.DataAnnotations;

namespace FribergAdminWebApi.Data.Dto
{
    public class WorkEntryUpdateDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan WorkDuration { get; set; }
    }
}
