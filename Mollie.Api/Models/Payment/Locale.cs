﻿using System.Runtime.Serialization;

namespace Mollie.Api.Models.Payment {
    public enum Locale {
        [EnumMember(Value = "de")]
        DE,
        [EnumMember(Value = "en")]
        EN,
        [EnumMember(Value = "es")]
        ES,
        [EnumMember(Value = "fr")]
        FR,
        [EnumMember(Value = "be")]
        BE,
        [EnumMember(Value = "be-fr")]
        BEFR,
        [EnumMember(Value = "nl")]
        NL
    }
}
