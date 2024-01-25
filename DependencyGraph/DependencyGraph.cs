// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{


    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        // In dependentGroup the key is dependees and value is hashset of dependents
        private Dictionary<string, HashSet<string>> dee_dentGroup;

        //In dependeeGroup the key is dependents and value is hashset of dependees
        private Dictionary<string, HashSet<string>> dent_deeGroup;

        //The size of the relations
        int numOfRelations;

        /// <summary>
        /// Creates an empty DependencyGraph by creating the empty dee_dentGroup
        /// and dent_deeGroup dictionary and set num of relations as 0
        /// </summary>
        public DependencyGraph()
        {
            dee_dentGroup = new Dictionary<string, HashSet<string>>();
            dent_deeGroup = new Dictionary<string, HashSet<string>>();
            numOfRelations = 0;
        }


        /// <summary>
        /// return how many relations exist in the dependency graph
        /// </summary>
        public int Size
        {
            get { return numOfRelations; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (!dent_deeGroup.ContainsKey(s))
                {
                    return 0;
                }
                else
                {
                    return dent_deeGroup[s].Count;
                }
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            return dee_dentGroup.ContainsKey(s) && dee_dentGroup[s].Count > 0;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return dent_deeGroup.ContainsKey(s) && dent_deeGroup[s].Count > 0;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dee_dentGroup.ContainsKey(s))
            {
                return dee_dentGroup[s];
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dent_deeGroup.ContainsKey(s))
            {
                return dent_deeGroup[s];
            }
            else
            {
                return new HashSet<string>();
            }
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (!dee_dentGroup.ContainsKey(s))
            {
                dee_dentGroup.Add(s, new HashSet<string>());
            }

            if (!dent_deeGroup.ContainsKey(t))
            {
                dent_deeGroup.Add(t, new HashSet<string>());
            }

            if (dent_deeGroup[t].Add(s) && dee_dentGroup[s].Add(t))
            {
                numOfRelations++;
            }

        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"> 
        ///     dependee s 
        /// </param>
        /// <param name="t"> 
        ///     dependent t
        /// </param>
        public void RemoveDependency(string s, string t)
        {
            if (!dee_dentGroup.ContainsKey(s))
            {
                return;
            }

            if (!dent_deeGroup.ContainsKey(t))
            {
                return;
            }

            if (dent_deeGroup[t].Remove(s) && dee_dentGroup[s].Remove(t))
            {
                numOfRelations--;
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (!dee_dentGroup.ContainsKey(s))
            {
                dee_dentGroup.Add(s, new HashSet<string>());
            }
            foreach (string oldDent in dee_dentGroup[s])
            {
                RemoveDependency(s, oldDent);
            }

            foreach (string newDent in newDependents)
            {
                AddDependency(s, newDent);
            }
        }

        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (!dent_deeGroup.ContainsKey(s))
            {
                dent_deeGroup.Add(s, new HashSet<string>());
            }
            foreach (string oldDee in dent_deeGroup[s])
            {
                RemoveDependency(oldDee, s);
            }

            foreach (string newDee in newDependees)
            {
                AddDependency(newDee, s);
            }
        }
    }
}
