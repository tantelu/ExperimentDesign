using System;

namespace ExperimentDesign.General
{
    public class Design<T>
    {
        public string DesignName { get; private set; }

        public bool IsDesign { get; private set; }

        public T Value { get; private set; }

        public Design(T t, string designName)
        {
            Value = t;
            IsDesign = true;
            DesignName = designName;
        }

        public Design(string designName)
        {
            Value = default(T);
            IsDesign = true;
            DesignName = designName;
        }

        public Design(T t)
        {
            DesignName = string.Empty;
            IsDesign = false;
            Value = t;
        }

        public static implicit operator Design<T>(T value)
        {
            return new Design<T>(value);
        }

        public static implicit operator T(Design<T> value)
        {
            return value.Value;
        }
    }
}
