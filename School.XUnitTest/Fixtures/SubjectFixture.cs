using School.Domain.Entities;

namespace School.Tests.Fixtures
{
    public class SubjectFixture
    {
        public static Subject CreateValidSubject(
      string nameEn = "Mathematics",
      string nameAr = "رياضيات",
      int period = 3)
        {
            return new Subject
            {
                SubjectNameEn = nameEn,
                SubjectNameAr = nameAr,
                Period = period
            };
        }

        public static List<Subject> CreateSubjectList(int count)
        {
            var subjects = new List<Subject>();

            for (int i = 1; i <= count; i++)
            {
                subjects.Add(new Subject
                {
                    SubjectNameEn = $"Subject {i}",
                    SubjectNameAr = $"مادة {i}",
                    Period = i
                });
            }

            return subjects;
        }
    }
}
