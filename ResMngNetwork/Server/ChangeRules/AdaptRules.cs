using DataSerailizer;
using Server.DSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.ChangeRules
{
    public class AdaptRules
    {
        public AdaptRules()
        {

        }

        public static object AdaptRulesForCSharp(DBData nData, ProposalCause pCause, ProposalType pType, string condition, string value)
        {
            if (condition.ToLower().Equals("nonempty"))
            {
                return !string.IsNullOrEmpty(value);
            }
            if (condition.ToLower().Equals("unique") & pCause == ProposalCause.NewPackage)
            {
                object result = null;
                foreach(PackageData pD in nData.PkgData)
                {
                    if(pD.PkgName.ToLower().Equals(value.ToLower()))
                    {
                        result = pD;
                    }
                }
                if (result == null)
                    return true;
                else
                    return false;
            }
            if (condition.ToLower().Equals("unique") & pCause == ProposalCause.NewClass)
            {
                object result = null;
                foreach(EntityData eD in nData.ClassData)
                {
                    if(eD.ETName.ToLower().Equals(value.ToLower()))
                    {
                        result = eD;
                    }
                }
                if (result == null)
                    return true;
                else
                    return false;
            }
            if (condition.ToLower().Equals("unique") & pCause == ProposalCause.NewProperty)
            {
                object result = null;
                foreach (EntityData eD in nData.PropertyData)
                {
                    if (eD.ETName.ToLower().Equals(value.ToLower()))
                    {
                        result = eD;
                    }
                }
                if (result == null)
                    return true;
                else
                    return false;
            }
            return null;
        }
    }
}
