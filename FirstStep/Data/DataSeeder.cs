using FirstStep.Models.DTOs;
using FirstStep.Services;

namespace FirstStep.Data
{
    public class DataSeeder
    {
        private readonly IAdvertisementService _advertisementService;

        public DataSeeder(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        public async Task SeedAdvertisements(int count)
        {
            // random advertisement values

            // random job titles
            string[] jobTitles = {
                "Software Engineer", "Senior Software Engineer", "Dynamics 365 and C# Developer", "Quality Assurance Engineer", "QA Analyst", "Solution Architect", "DevOps Engineer", "Frontend Developer", "Backend Developer"
            };

            // random countries and cities
            Dictionary<string, string[]> countries = new Dictionary<string, string[]>()
            {
                { "Sri Lanka", new string[] {"Colombo", "Kandy", "Matara", "Kalutara", "Gampaha", "Moratuwa" } },
                { "India", new string[] {"Mumbai", "Delhi", "Bangalore", "Hyderabad" } },
                { "United States", new string[] {"New York", "Los Angeles", "Chicago", "Houston" } },
                { "Australia", new string[] {"Sydney", "Melbourne", "Brisbane", "Perth" } },
            };

            string[] cities = { "Colombo", "Kandy", "Matara", "Kalutara", "Gampaha", "Moratuwa", "Galle"};

            // random employment types
            string[] employmentTypes = { "Full-time", "Part-time", "Contract", "Internship", "Temporary", "Volunteer" };

            // random arrangements
            string[] arrangements = { "Remote", "On-site", "Hybrid" };

            // random experiences
            string[] experiences = { "Internship", "Entry Level", "Associate", "Mid Level", "Senior Level", "Experienced" };

            // random job keywords
            string[] keywords = { "software engineer", "web developer", "full stack developer", "software developer" };

            //string[] keywords = { "ai", "machine learning", "data science", "devop", "big data", "deep learning", "computer vision", "python", "software engineer", "ai engineer" };
            string[] skills = { "C#", "ASP.NET", "Unit Testing", "Agile", "Continuous Integration", "RESTful APIs" };

            // random salary
            float[] salaries = { 6500f, 2500f, 2550f, 1500f };

            // random hr managers
            int[] hrmanager = { 20, 7, 8 };

            // sample description
            string description = @"
				<h2>
					<strong>
						Overview
					</strong>
				</h2>
				<p style=""letter-spacing: 0.214286px;"">
					As a Machine Learning Engineer, you’ll play a crucial role in designing, developing, and implementing machine learning models and algorithms. Your work will directly impact our organization’s products and services, driving innovation and improving user experiences.
				</p>
				<h2>
					<strong>
						Qualifications
					</strong>
				</h2>
				<ul>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							A strong background in machine learning, deep learning, and statistical modeling.
						</span>
					</li>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Proficiency in programming languages such as Python, R, or Julia.
						</span>
					</li>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Experience with popular machine learning libraries (e.g., TensorFlow, PyTorch, scikit-learn).
						</span>
					</li>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Knowledge of data preprocessing, feature engineering, and model evaluation techniques.
						</span>
					</li>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Strong problem-solving skills and the ability to collaborate with cross-functional teams.
						</span>
					</li>
				</ul>	
				<h2>
					<strong>
						Responsibilities
					</strong>
				</h2>
				<ul>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Developing and implementing machine learning models for various applications (e.g., natural language processing, computer vision, recommendation systems).
						</span>
					</li>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Collecting, cleaning, and analyzing large datasets to extract meaningful insights.
						</span>
					</li>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Collaborating with data scientists, software engineers, and product managers to define project goals and requirements.
						</span>
					</li>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Optimizing and fine-tuning models for performance and scalability.
						</span>
					</li>
					<li style="""">
						<span style=""letter-spacing: 0.214286px;"">
							Staying up-to-date with the latest advancements in machine learning and AI.
						</span>
					</li>
				</ul>";

			Random random = new Random();

			List<string> selected_keywords;
			List<string> selected_skills;

            for (int i = 0; i < count; i++)
			{
				selected_keywords = new List<string>();
				selected_skills = new List<string>();

				for (int j = 0; j < random.Next(1, 4); j++)
				{
					string keyword = keywords[random.Next(keywords.Length)];

					if (!selected_keywords.Contains(keyword))
					{
                        selected_keywords.Add(keyword);
                    }
					else
					{
						j--;
					}
				}

				for (int j = 0; j < random.Next(1, 6); j++)
				{
					string skill = skills[random.Next(skills.Length)];

					if (!selected_skills.Contains(skill))
					{
                        selected_skills.Add(skill);
                    }
                    else
					{
                        j--;
                    }
				}

                AddAdvertisementDto newAd = new AddAdvertisementDto
				{
                    job_number = i + 1,
                    title = jobTitles[random.Next(jobTitles.Length)],
                    country = "Sri Lanka",
                    city = cities[random.Next(cities.Length)],
                    employeement_type = employmentTypes[random.Next(employmentTypes.Length)],
                    arrangement = arrangements[random.Next(arrangements.Length)],
                    experience = experiences[random.Next(experiences.Length)],
                    salary = salaries[i % salaries.Length],
                    currency_unit = "USD",
                    submission_deadline = DateTime.Now.AddDays(90),
                    job_description = description,
                    hrManager_id = hrmanager[random.Next(hrmanager.Length)],
                    field_id = 1,
                    keywords = selected_keywords,
                    reqSkills = selected_skills
                };

				await _advertisementService.Create(newAd);
            }
        }
    }
}
