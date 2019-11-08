namespace EFCore.Common
{
    public static class Extensions
    {
        public static SortOrder Reverse(this SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return SortOrder.Descending;
            }
            else
            {
                return SortOrder.Ascending;
            }
        }

        public static SortOrder ToSortOrder(this string sortOrder)
        {
            if (string.IsNullOrEmpty(sortOrder) || string.IsNullOrWhiteSpace(sortOrder))
            {
                return SortOrder.Ascending;
            }
            else if (sortOrder.ToLower().Equals("descending") || sortOrder.ToLower().Equals("desc"))
            {
                return SortOrder.Descending;
            }
            else
            {
                return SortOrder.Ascending;
            }
        }

        public static Gender ToGenderEnum(this string gender)
        {
            if (string.IsNullOrEmpty(gender) || gender.Trim().ToLower().Equals("female"))
            {
                return Gender.Female;
            }

            return Gender.Male;
        }

        public static Gender? ToNullableGenderEnum(this string gender)
        {
            if (!string.IsNullOrEmpty(gender))
            {
                string value = gender.Trim().ToLower();
                if (value.Equals("female"))
                {
                    return Gender.Female;
                }
                else if (value.Equals("male"))
                {
                    return Gender.Male;
                }
            }

            return null;
        }

        public static bool? ToNullableBool(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim().ToLower();
                if (value.Equals("true") || value.Equals("yes") || value.Equals("1"))
                {
                    return true;
                }
                else if (value.Equals("false") || value.Equals("no") || value.Equals("0"))
                {
                    return false;
                }
            }

            return null;
        }
    }
}
