using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyGoidaList.Models
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsMainCharacter { get; set; }

        public float PopularityScore { get; set; }

        [Range(0, 1000)]
        public long ScreenTime { get; set; }

        public decimal Rating { get; set; }

        public double Height { get; set; }

        public DateTime BirthDate { get; set; }

        [NotMapped]
        public TimeSpan AverageScreenTimePerEpisode { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [ForeignKey("Title")]
        public int TitleId { get; set; }

        public virtual Title Title { get; set; }
    }
}
