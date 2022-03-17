using System;

namespace ExperimentDesign.General
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GroupAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class GroupListAttribute : Attribute
    {

    }
}
