using FirstStep.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstStep.Validation
{
    public static class AdvertisementValidation
    {
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
            if (advertisement.current_status != Advertisement.Status.active.ToString())
            {
                return false;
            }

            return true;
        }

        public static bool IsHold(Advertisement advertisement)
        {
            if (advertisement.current_status != Advertisement.Status.hold.ToString())
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

        public static void IsSutableForApply(Advertisement? advertisement)
        {
            // validate advertisement
            if (advertisement is null)
            {
                throw new InvalidDataException("Advertisement not found.");
            }
            else if (IsExpired(advertisement))
            {
                throw new InvalidDataException("Advertisement is expired.");
            }
            else if (!IsActive(advertisement))
            {
                throw new InvalidDataException("Advertisement is not active.");
            }
        }

        public static void CheckStatus(string status)
        {
            if (status == "all")
            {
                return;
            }
            if (!Enum.TryParse<Advertisement.Status>(status, out _))
            {
                throw new InvalidDataException("Invalid status.");
            }
        }
    }
}
