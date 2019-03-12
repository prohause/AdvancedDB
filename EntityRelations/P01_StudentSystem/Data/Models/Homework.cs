using System;

namespace P01_StudentSystem.Data.Models
{
    public class Homework
    {
        public Homework()
        {
            SubmissionTime = DateTime.Now;
        }

        public int HomeworkId { get; set; }

        public string Content { get; set; }

        public ContentType ContentType { get; set; }

        public DateTime SubmissionTime { get; }

        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}