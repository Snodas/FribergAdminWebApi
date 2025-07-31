using System.ComponentModel.DataAnnotations;

namespace FribergAdminWebApi.Data.Dto
{
    public class WorkEntryCreateDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan WorkDuration { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
