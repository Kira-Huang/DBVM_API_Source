using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DBVM_API
{
    public enum enum_藥局位置
    {
        [Description("PHR")]
        [Display(Name = "中央藥局")]
        中央藥局,
        [Description("PHRO")]
        [Display(Name = "門診藥局")]
        門診藥局,
        [Description("PHRE")]
        [Display(Name = "急診藥局")]
        急診藥局,
        [Description("PHR6")]
        [Display(Name = "二醫藥局")]
        二醫藥局,
        [Description("PHRN")]
        [Display(Name = "北院區藥局")]
        北院區藥局
    }

    public enum enum_調劑時間
    {
        [Description("AM")]
        [Display(Name = "上午")]
        上午,
        [Description("PM")]
        [Display(Name = "下午")]
        下午
    }

    public enum enum_藥袋類別
    {
        [Description("discharge")]
        [Display(Name = "出院帶藥")]
        出院帶藥,
        [Description("daytime")]
        [Display(Name = "日間帶藥")]
        日間帶藥,
        [Description("smallbag")]
        [Display(Name = "小藥袋")]
        小藥袋
    }

    public enum enum_登入類型
    {
        [Description("Auth")]
        [Display(Name = "卡號密碼登入")]
        卡號密碼登入,
        [Description("NFC")]
        [Display(Name = "識別證登入")]
        識別證登入
    }

    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            return attr?.Description ?? value.ToString();
        }

        public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();

            return attribute?.Name ?? value.ToString();
        }

        public static bool TryToEnumFromDescription<TEnum>(this string description, out TEnum result) where TEnum : Enum
        {
            result = default;

            if (string.IsNullOrWhiteSpace(description))
                return false;

            foreach (var field in typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = field.GetCustomAttribute<DescriptionAttribute>();
                if (attr != null && attr.Description == description)
                {
                    result = (TEnum)field.GetValue(null);
                    return true;
                }
            }

            return false;
        }
    }
}
