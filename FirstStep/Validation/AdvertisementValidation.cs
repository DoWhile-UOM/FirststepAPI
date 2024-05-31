using FirstStep.Models;

namespace FirstStep.Validation
{
    public static class AdvertisementValidation
    {
        public enum Status { active, hold, closed }

        public static bool IsExpired(Advertisement advertisement)
        {
            if (advertisement.submission_deadline == null)
            {
                return false;
            }

            return DateTime.Now > advertisement.submission_deadline;
        }

        public static bool IsActive(Advertisement advertisement)
        {
            if (advertisement.current_status != Status.active.ToString())
            {
                return false;
            }

            return true;
        }

        public static bool IsSaved(Advertisement advertisement, Seeker seeker)
        {
            if (advertisement.savedSeekers is null || !advertisement.savedSeekers.Contains(seeker))
            {
                return false;
            }

            return true;
        }

        public static void CheckStatus(string status)
        {
            if (status == "all")
            {
                return;
            }
            if (!Enum.TryParse<Status>(status, out _))
            {
                throw new InvalidDataException("Invalid status.");
            }
        }
    }
}
