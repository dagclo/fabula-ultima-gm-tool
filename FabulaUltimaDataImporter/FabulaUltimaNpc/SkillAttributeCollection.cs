
using Newtonsoft.Json.Linq;
using System.Diagnostics.CodeAnalysis;


namespace FabulaUltimaNpc
{
    public class SkillAttributeCollection
    {


        private IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

        public SkillAttributeCollection(IDictionary<string, string>? dataDictionary)
        {
            if (dataDictionary is null) throw new ArgumentNullException(nameof(dataDictionary));
            foreach (var pair in dataDictionary)
            {
                this[pair.Key] = pair.Value;
            }
        }

        public SkillAttributeCollection() { }

        public IDictionary<string, string> DataDictionary
        {
            get
            {
                // ensure set values in additional data
                if (OtherKnownSkillsRequired != null) this[nameof(OtherKnownSkillsRequired)] = string.Join(",", OtherKnownSkillsRequired);
                if (FreeSpecies != null) this[nameof(FreeSpecies)] = string.Join(",", FreeSpecies);
#pragma warning disable CS8601 // Possible null reference assignment.
                if (IsSpecialAttack != null) this[nameof(IsSpecialAttack)] = IsSpecialAttack.ToString();
                if (Ordering != null) this[nameof(Ordering)] = Ordering.ToString();
#pragma warning restore CS8601 // Possible null reference assignment.

                return _additionalData.ToDictionary(p => p.Key, p => p.Value.ToString());
            }
        }

        public string? this[string key]
        {
            get
            {
                if (TryGetValue(key, out string? value))
                {
                    return value;
                }
                throw new SkillAttributeCollectionExceptionKeyNotFound(key);
            }
            set
            {   
                if (value != null)
                {
                    if (string.Equals(key, nameof(OtherKnownSkillsRequired)))
                    {
                        var guidSplit = value.Split(',').Select(v => Guid.Parse(v));
                        OtherKnownSkillsRequired = guidSplit.ToArray();
                    }
                    else if (string.Equals(key, nameof(FreeSpecies)))
                    {
                        var guidSplit = value.Split(',').Select(v => Guid.Parse(v));
                        FreeSpecies = guidSplit.ToArray();
                    }
                    else if (string.Equals(key, nameof(IsSpecialAttack)))
                    {
                        IsSpecialAttack = bool.Parse(value);
                    }
                    else if (string.Equals(key, nameof(Ordering)))
                    {
                        Ordering = int.Parse(value);
                    }

                    _additionalData[key] = JToken.FromObject(value);
                }
                else
                {
                    _additionalData.Remove(key);
                }
            }
        }

        public ICollection<Guid>? OtherKnownSkillsRequired { get; set; }
        public ICollection<Guid>? FreeSpecies { get; set; }
        public bool? IsSpecialAttack { get; set; }
        public int? Ordering { get; set; } = null;

        public bool Any()
        {
            if (IsSpecialAttack.HasValue) return true;
            if (OtherKnownSkillsRequired?.Any() == true) return true;
            if (FreeSpecies?.Any() == true) return true;
            if (Ordering.HasValue) return true;
            return _additionalData.Any();
        }

        public bool ContainsKey(string key)
        {
            return TryGetValue(key, out string? _);
        }

        public bool TryGetValue(string key, out string? value)
        {
            value = null;
            if (string.Equals(key, nameof(OtherKnownSkillsRequired), StringComparison.InvariantCultureIgnoreCase) && OtherKnownSkillsRequired?.Any() == true)
            {
                value = string.Join(",", OtherKnownSkillsRequired);
                return true;
            }

            if (string.Equals(key, nameof(IsSpecialAttack), StringComparison.InvariantCultureIgnoreCase))
            {
                value = IsSpecialAttack?.ToString();
                return true;
            }

            if (string.Equals(key, nameof(FreeSpecies), StringComparison.InvariantCultureIgnoreCase) && FreeSpecies?.Any() == true)
            {
                value = string.Join(",", FreeSpecies);
                return true;
            }

            if (string.Equals(key, nameof(Ordering), StringComparison.InvariantCultureIgnoreCase))
            {
                value = Ordering?.ToString();
                return true;
            }

            if (_additionalData.TryGetValue(key, out var token))
            {
                value = (string?)token;
                return true;
            }
            return false;
        }

        public SkillAttributeCollection Clone()
        {
            return new SkillAttributeCollection(DataDictionary)
            {
                OtherKnownSkillsRequired = OtherKnownSkillsRequired,
                IsSpecialAttack = IsSpecialAttack,
                FreeSpecies = FreeSpecies,
                Ordering = Ordering,
            };
        }
    }
}
