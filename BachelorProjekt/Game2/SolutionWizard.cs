﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Game2.Component;
using Game2.Component.PlayableComponent.ConnectorCross;
using Game2.Component.PlayableComponent.Divide;
using Game2.Component.PlayableComponent.Filter;
using Game2.Component.PlayableComponent.Minus;
using Game2.Component.PlayableComponent.Multiply;
using Game2.Component.PlayableComponent.Plus;
using Game2.Component.PlayableComponent.PlusMinusX;
using Game2.Component.PlayableComponent.Splitter;

namespace Game2
{
    public class SolutionWizard
    {

        private const int DefaultMaxSpecificCompUsable = 3;
        private Random RNG = new Random();
        /// <summary>
        ///  <para>-----</para>
        /// <para>Generates a random level. No garauntees are made in regards to difficulty.</para>
        /// <para>-----</para>
        /// <para>0 is easy, 1 is medium and 2 is hard. Other values are considering as 2.</para>
        /// <para>-----</para>
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public Solution GetRandomLevel(int difficulty)
        {
            if (difficulty > 2 || difficulty < 0)
                difficulty = 2;

            Solution result = new Solution();
            List<int> boardIns = new List<int>();
            int NumberOfCompsToApply = -1;
            string humanSolution = "";
            string humanCompName = "";
                
            switch (difficulty)
            {
                case 0:
                    NumberOfCompsToApply = RNG.Next(1, 4);

                    boardIns.Add(RNG.Next(1, 40));
                    boardIns.Add(RNG.Next(12, 54));
                    if (RNG.Next(0, 4) <= 2)
                    {
                        boardIns.Add(RNG.Next(23, 36));
                        NumberOfCompsToApply++;
                    }
                    break;
                case 1:
                    NumberOfCompsToApply = RNG.Next(3, 5);

                    boardIns.Add(RNG.Next(60, 120));
                    boardIns.Add(RNG.Next(1, 99));
                    boardIns.Add(RNG.Next(32, 78));
                    if (RNG.Next(0,3) == 0)
                    {
                        boardIns.Add(RNG.Next(79, 169));
                        NumberOfCompsToApply++;
                    }
                    break;
                case 2:
                default:
                    NumberOfCompsToApply = RNG.Next(6, 8);

                    //boardIns.Add(RNG.Next(60, 194));
                    boardIns.Add(RNG.Next(1, 99));
                    boardIns.Add(RNG.Next(50, 130));
                    boardIns.Add(RNG.Next(32, 112));
                    if (RNG.Next(0,2) == 0)
                    {
                        boardIns.Add(RNG.Next(2, 220));
                        NumberOfCompsToApply++;
                    }
                    break;
            }

            MathComponentModel CompToApply;
            List<MathComponentModel> Toolbox = new List<MathComponentModel>()
            {
                new PlusComponentModel(null, 0),
                new MinusComponentModel(null, 0),
                new MultiplyComponentModel(null, 0),
                //new DivideComponentModel(null, 0),
                new ConnectorCrossComponentModel(null, 0),
                new FilterComponentModel(10, null, 0),
                new FilterComponentModel(100, null, 0),

                new SplitterComponentModel(null, 0),

                new PlusMinusXComponentModel(10, PlusMinusXComponentModel.PlusFunc, null, 0),
                new PlusMinusXComponentModel(100, PlusMinusXComponentModel.PlusFunc, null, 0),

                new PlusMinusXComponentModel(10, PlusMinusXComponentModel.MinusFunc, null, 0),
                new PlusMinusXComponentModel(100, PlusMinusXComponentModel.MinusFunc, null, 0),
            };
            //filter1, plus1 and minus1 has been removed. They need careful consideration to be added to a level

            result.Toolbox.Add(LevelSpecification.ComponentType.connectorcross);
            result.BoardIns.AddRange(boardIns);

            int boardInIndex;
            bool got100 = false;
            int lowestOpenConnector = 999999999, indexOfLowestOpenconnector = 0;
            List<int> MathFuncOutput;
            for (int i = 1; i < NumberOfCompsToApply; i++)
            {
                if (boardIns.Count < 2)
                    CompToApply = Toolbox[RNG.Next(2, Toolbox.Count)];
                else
                    CompToApply = Toolbox[RNG.Next(0, Toolbox.Count)];

                if (CompToApply is FilterComponentModel)
                {
                    if (((FilterComponentModel)CompToApply).Filters == 100)
                    {
                        foreach (int j in boardIns)
                        {
                            if (j > 100)
                                got100 = true;
                        }
                        if (got100 == false)
                            CompToApply = new FilterComponentModel(10, null, 0);
                    }
                }

                if (CompToApply is ConnectorCrossComponentModel)
                    i--;
                else
                {
                    #region human solution
                    if (CompToApply is PlusComponentModel)
                        humanSolution = humanSolution + "Brug |  Plus  | på ";
                    else if (CompToApply is MinusComponentModel)
                        humanSolution = humanSolution + "Brug |  Minus  | på ";
                    else if (CompToApply is MultiplyComponentModel)
                        humanSolution = humanSolution + "Brug |  Gange 10  | på ";
                    else if (CompToApply is DivideComponentModel)
                        humanSolution = humanSolution + "Brug |  Divider med 10  | på ";
                    else if (CompToApply is SplitterComponentModel)
                        humanSolution = humanSolution + "Brug |  Halver  | på ";
                    else if (CompToApply is FilterComponentModel)
                        humanSolution = humanSolution + "Brug |  Filter" + ((FilterComponentModel)CompToApply).Filters + "  | på ";
                    else if (CompToApply is PlusMinusXComponentModel)
                    {
                        if (((PlusMinusXComponentModel)CompToApply).PlusOrMinus == PlusMinusXComponentModel.PlusFunc)
                            humanSolution = humanSolution + "Brug |  Plus" + ((PlusMinusXComponentModel)CompToApply).XX + "  | på ";
                        else
                            humanSolution = humanSolution + "Brug |  Minus" + ((PlusMinusXComponentModel)CompToApply).XX + "  | på ";
                    }
                    #endregion
                }

                if (result.Toolbox.Contains(GetTypeFromModel(CompToApply)) == false)
                    result.Toolbox.Add(GetTypeFromModel(CompToApply));

                if (CompToApply is Component2i1oModel || CompToApply is Component2i2oModel)
                {
                    boardInIndex = RNG.Next(0, boardIns.Count - 1);
                    if (!(CompToApply is ConnectorCrossComponentModel))
                        humanSolution = humanSolution + "" + (boardIns[boardInIndex]) + " og " + (boardIns[boardInIndex + 1]) + ".\n";
                    MathFuncOutput = CompToApply.MathFunc(new int[2] { boardIns[boardInIndex], (boardIns[boardInIndex + 1]) });
                }
                else
                {
                    boardInIndex = RNG.Next(0, boardIns.Count);
                    if (CompToApply is FilterComponentModel)
                    {
                        if (((FilterComponentModel)CompToApply).Filters == 100)
                        {
                            while(boardIns[boardInIndex] < 100)
                                boardInIndex = RNG.Next(0, boardIns.Count);
                        }
                    }
                    else if (CompToApply is MultiplyComponentModel && boardIns[boardInIndex] > 99)
                    {
                        for (int j = 0; j < boardIns.Count; j++)
                        {
                            if (boardIns[j] < lowestOpenConnector)
                            {
                                lowestOpenConnector = boardIns[j];
                                indexOfLowestOpenconnector = j;
                            }
                        }
                        boardInIndex = indexOfLowestOpenconnector;
                        indexOfLowestOpenconnector = 0;
                        lowestOpenConnector = 999999999;
                    }
                    humanSolution = humanSolution + "" + (boardIns[boardInIndex]) + ".\n";
                    MathFuncOutput = CompToApply.MathFunc(new int[2] { boardIns[boardInIndex], 0 });
                }


                boardIns[boardInIndex] = MathFuncOutput[0];

                if (CompToApply is Component2i1oModel || CompToApply is Component2i2oModel)
                    boardIns.RemoveAt(boardInIndex + 1);
                if (CompToApply is Component2oModel)
                    boardIns.Insert(boardInIndex + 1, MathFuncOutput[1]);
            }

            if (difficulty == 0)
            {
                while (boardIns.Count > 2)
                {
                    boardIns.RemoveAt(RNG.Next(0, boardIns.Count));
                }
            }
            else if (difficulty == 1)
            {
                while (boardIns.Count > 4)
                {
                    boardIns.RemoveAt(RNG.Next(0, boardIns.Count));
                }
            }
            else
            {
                while (boardIns.Count > 7)
                {
                    boardIns.RemoveAt(RNG.Next(0, boardIns.Count));
                }
            }
             
            result.BoardOuts.AddRange(boardIns);
            result.HumanSolution = humanSolution;

            return result;
        }

        #region Used for testing if levels have an appropriate number of ways to achieve the solution
        //public List<Solution> TestAllRandomDiffs(List<int> BoardIns, int TargetNumberOfBoardOuts, List<MathComponentModel> AvailableComponents, List<MathComponentModel> FixedComponents, int MaxCompsPossibleForViableSolution, int MinCompsNeededForViableSolution, List<LevelSpecification.ComponentType> ComponentsRequiredForSolution, int MaxOfEachSpecificCompAllowed = DefaultMaxSpecificCompUsable)
        //{
        //    List<Solution> result = new List<Solution>();
        //    List<List<int>> NewBoardIns = new List<List<int>>();
        //    List<int> buffer = new List<int>();
        //    for (int j = -5; j<6; j++)
        //    {
        //        foreach (int i in BoardIns)
        //        {
                    
        //        }
        //    }

        //    return result;
        //}
        //private List<int> GenerateTestCase(List<int>)
        //{
        //    return null;
        //}
        //private Solution TestOneRandomDiff(List<int> BoardIns, int TargetNumberOfBoardOuts, List<MathComponentModel> AvailableComponents, List<MathComponentModel> FixedComponents, int MaxCompsPossibleForViableSolution, int MinCompsNeededForViableSolution, List<LevelSpecification.ComponentType> ComponentsRequiredForSolution, int RandomDiff, int MaxOfEachSpecificCompAllowed = DefaultMaxSpecificCompUsable)
        //{
        //    List<int> NewBoardIns = new List<int>();
        //    foreach (int i in BoardIns)
        //        NewBoardIns.Add(i + RandomDiff);

        //    return GenerateBoardOutsForGivenBoardIns(NewBoardIns, TargetNumberOfBoardOuts, AvailableComponents, FixedComponents, MaxCompsPossibleForViableSolution, MinCompsNeededForViableSolution, ComponentsRequiredForSolution, MaxOfEachSpecificCompAllowed);
        //}
        #endregion

        /// <summary>
        /// <para>-----</para>
        /// <para>-----</para>
        /// <para>DON'T USE</para>
        /// <para>-----</para>
        /// <para>-----</para>
        /// <para>Generates the integers which are used for the BoardOuts, given a set of integers describing the BoardIns which are first randomized slightly.</para>
        /// <para>As connector crosses can be used without changing the result, the algorithm adds MaxOfEachSpecificCompAllowed to the user specified MinCompsNeededForViableSolution to prevent a solution to be too easy becayse only connector crosses is needed. </para>
        /// <para>This presumes that Connector cross is avaible in the level, so if this is not the case please substract the value of MaxOfEachSpecificCompAllowed from MinCompsNeededForViableSolution.</para>
        /// <para>-----</para>
        /// </summary>
        /// <param name="BoardIns"></param>
        /// <param name="TargetNumberOfBoardOuts"></param>
        /// <param name="AvailableComponents"></param>
        /// <param name="FixedComponents"></param>
        /// <param name="MaxCompsPossibleForViableSolution"></param>
        /// <param name="MinCompsNeededForViableSolution"></param>
        /// <param name="ComponentsRequiredForSolution"></param>
        /// <param name="MaxOfEachSpecificCompAllowed"></param>
        /// <returns></returns>
        public Solution GenerateBoardOutsForGivenBoardInsWithRandomDiff(List<int> BoardIns, int TargetNumberOfBoardOuts, List<MathComponentModel> AvailableComponents, List<MathComponentModel> FixedComponents, int MaxCompsPossibleForViableSolution, int MinCompsNeededForViableSolution, List<LevelSpecification.ComponentType> ComponentsRequiredForSolution, int MaxOfEachSpecificCompAllowed = DefaultMaxSpecificCompUsable)
        {
            int MaxNegDiff = 5, MinNegDiff = 0, MinPosDiff = 0, MaxPosDiff = 5;
            Random RNG = new Random();
            List<int> NewBoardIns = new List<int>();
            foreach (int i in BoardIns)
                NewBoardIns.Add((i + RNG.Next(MinPosDiff, MaxPosDiff+1)) - RNG.Next(MinNegDiff, MaxNegDiff+1));

            return GenerateBoardOutsForGivenBoardIns(NewBoardIns, TargetNumberOfBoardOuts, AvailableComponents, FixedComponents, MaxCompsPossibleForViableSolution, MinCompsNeededForViableSolution, ComponentsRequiredForSolution, MaxOfEachSpecificCompAllowed);
        }

        /// <summary>
        /// <para>-----</para>
        /// <para>Generates the integers which are used for the BoardOuts, given a set of integers describing the BoardIns.</para>
        /// <para>-----</para>
        /// <para>THE FOLLOWING DON'T COUNT ANYMORE</para>
        /// <para>-----</para>
        /// <para>As connector crosses can be used without changing the result, the algorithm adds MaxOfEachSpecificCompAllowed to the user specified MinCompsNeededForViableSolution to prevent a solution to be too easy becayse only connector crosses is needed. </para>
        /// <para>This presumes that Connector cross is avaible in the level, so if this is not the case please substract the value of MaxOfEachSpecificCompAllowed from MinCompsNeededForViableSolution.</para>
        /// <para>-----</para>
        /// </summary>
        /// <param name="BoardIns"></param>
        /// <param name="TargetNumberOfBoardOuts"></param>
        /// <param name="AvailableComponents"></param>
        /// <param name="FixedComponents"></param>
        /// <param name="MaxCompsPossibleForViableSolution"></param>
        /// <param name="MinCompsNeededForViableSolution"></param>
        /// <param name="ComponentsRequiredForSolution"></param>
        /// <param name="MaxOfEachSpecificCompAllowed"></param>
        /// <returns></returns>
        public Solution GenerateBoardOutsForGivenBoardIns(List<int> BoardIns, int TargetNumberOfBoardOuts, List<MathComponentModel> AvailableComponents, List<MathComponentModel> FixedComponents, int MaxCompsPossibleForViableSolution, int MinCompsNeededForViableSolution, List<LevelSpecification.ComponentType> ComponentsRequiredForSolution, int MaxOccurencesWanted, int MaxOfEachSpecificCompAllowed = DefaultMaxSpecificCompUsable)
        {
            int MinCompsForSolution = MinCompsNeededForViableSolution;//MaxOfEachSpecificCompAllowed + MinCompsNeededForViableSolution;

            HashSet<LevelSpecification.ComponentType> set = new HashSet<LevelSpecification.ComponentType>();
            foreach (MathComponentModel mcm in AvailableComponents)
            {
                set.Add(GetTypeFromModel(mcm));
            }
            if (set.Count < AvailableComponents.Count)
                throw new IndexOutOfRangeException("Duplicate element in toolbox found.");


            return GetSolution(BoardIns, TargetNumberOfBoardOuts, MinCompsForSolution, MaxCompsPossibleForViableSolution, FixedComponents, AvailableComponents, MaxOfEachSpecificCompAllowed, ComponentsRequiredForSolution, MaxOccurencesWanted);
        }

        private Solution GetSolution(List<int> BoardIns, int TargetNumberOfBoardOuts, int MinCompsForSolution, int MaxCompForSolution, 
            List<MathComponentModel> FixedComponents, List<MathComponentModel> Toolbox, int MaxOfEachSpecificCompAllowed, 
            List<LevelSpecification.ComponentType> SolutionMustContain, int MaxOccurencesWanted)
        {            
            List<int> BoardOuts = new List<int>();
            bool GotExpanderComp = false, RequiredCompFound = false;

            #region consistency checks
            foreach (LevelSpecification.ComponentType type in SolutionMustContain)
            {
                RequiredCompFound = false;
                foreach (MathComponentModel mcm in Toolbox)
                {
                    if ((GetTypeFromModel(mcm) == type))
                    {
                        RequiredCompFound = true;
                        break;
                    }
                }
                foreach (MathComponentModel mcm in FixedComponents)
                {
                    if ((GetTypeFromModel(mcm) == type))
                    {
                        RequiredCompFound = true;
                        break;
                    }
                }
                if (RequiredCompFound == false)
                    throw new IndexOutOfRangeException("Solution can't contain the required component: " + type.ToString());
            }
            foreach (MathComponentModel mcm in Toolbox)
            {
                mcm.Locked = false;
                foreach (MathComponentModel fmcm in FixedComponents)
                {
                    if (GetTypeFromModel(mcm) == GetTypeFromModel(fmcm))
                        throw new IndexOutOfRangeException("The toolbox can't contain component types that are also fixed: " + GetTypeFromModel(fmcm).ToString());
                }
            }
            if (TargetNumberOfBoardOuts > 8 || BoardIns.Count > 8)
                throw new IndexOutOfRangeException("BoardIn/BoardOut algorithm can't handle more than 8 Ins/Outs. Outs attempted: " + TargetNumberOfBoardOuts + ". Ins attempted: " + BoardIns.Count);
            #endregion

            List<MathComponentModel> buffer = new List<MathComponentModel>();
            buffer.AddRange(Toolbox);
            buffer.AddRange(FixedComponents);
            foreach (MathComponentModel mcm in buffer)
            {
                if (mcm.inLimit < mcm.outLimit)
                    GotExpanderComp = true;
            }

            Dictionary<LevelSpecification.ComponentType, int> FixedComponentsMaxOccurenceMapping = new Dictionary<LevelSpecification.ComponentType, int>();
            foreach (MathComponentModel mcm in FixedComponents)
            {
                FixedComponentsMaxOccurenceMapping.Add(GetTypeFromModel(mcm), 1);
            }

            bool DuplicateFixedCompFound = false;
            foreach (MathComponentModel mcm in FixedComponents)
            {
                mcm.Locked = true; //marking which component is Fixed
                foreach (MathComponentModel mcm2 in Toolbox)
                {
                    if (GetTypeFromModel(mcm) == GetTypeFromModel(mcm2))
                    {
                        FixedComponentsMaxOccurenceMapping[GetTypeFromModel(mcm)]++;
                        DuplicateFixedCompFound = true;
                        break;
                    }
                }
                if (DuplicateFixedCompFound == false)
                    Toolbox.Add(mcm);
                DuplicateFixedCompFound = false;
            }


            Dictionary<LevelSpecification.ComponentType, int> CompOccurenceMapping = new Dictionary<LevelSpecification.ComponentType, int>();
            foreach (MathComponentModel mcm in Toolbox)
            {
                CompOccurenceMapping.Add(GetTypeFromModel(mcm), 0);
            }

            List<int> NewBoardIns;
            List<MathComponentModel> NewToolbox;
            Dictionary<LevelSpecification.ComponentType, int> NewCompOccurenceMapping;
            Dictionary<LevelSpecification.ComponentType, int> NewFixedComponentsMaxOccurenceMapping;
            List<LevelSpecification.ComponentType> NewSolutionMustContain;

            List<int> IndiciesTried = new List<int>();
            int RandomCompIndex;
            while (IndiciesTried.Count < Toolbox.Count)
            {
                RandomCompIndex = PickRandomNumberBetweenXYExcludingZ(0, Toolbox.Count - 1, ref IndiciesTried);

                #region makes copies for the next recursion step
                NewBoardIns = new List<int>();
                foreach (int i in BoardIns)
                    NewBoardIns.Add(i);

                NewToolbox = new List<MathComponentModel>();
                foreach (MathComponentModel mcm2 in Toolbox)
                    NewToolbox.Add(mcm2);

                NewCompOccurenceMapping = new Dictionary<LevelSpecification.ComponentType, int>();
                foreach (LevelSpecification.ComponentType type in CompOccurenceMapping.Keys)
                    NewCompOccurenceMapping.Add(type, CompOccurenceMapping[type]);

                NewFixedComponentsMaxOccurenceMapping = new Dictionary<LevelSpecification.ComponentType, int>();
                foreach (LevelSpecification.ComponentType type in FixedComponentsMaxOccurenceMapping.Keys)
                    NewFixedComponentsMaxOccurenceMapping.Add(type, FixedComponentsMaxOccurenceMapping[type]);

                NewSolutionMustContain = new List<LevelSpecification.ComponentType>();
                foreach (LevelSpecification.ComponentType type in SolutionMustContain)
                    NewSolutionMustContain.Add(type);
                #endregion

                FindAllSolutionsFromGivenRoot(NewBoardIns, TargetNumberOfBoardOuts, GotExpanderComp,
                    NewCompOccurenceMapping, MinCompsForSolution, MaxCompForSolution, Toolbox[RandomCompIndex], NewToolbox,
                    new List<LevelSpecification.ComponentType>(), MaxOfEachSpecificCompAllowed,
                    NewFixedComponentsMaxOccurenceMapping, NewSolutionMustContain, MaxOccurencesWanted);
                IndiciesTried.Add(RandomCompIndex);
            }

            //foreach (MathComponentModel mcm in Toolbox)
            //{
            //    FindAllSolutionsFromGivenRoot(BoardIns, TargetNumberOfBoardOuts, GotExpanderComp,
            //        CompOccurenceMapping, MinCompsForSolution, MaxCompForSolution, mcm, Toolbox,
            //        new List<LevelSpecification.ComponentType>(), MaxOfEachSpecificCompAllowed,
            //        FixedComponentsMaxOccurenceMapping, SolutionMustContain, MaxOccurencesWanted);
            //}

            #region post-processing solutions to make them match TargetOut
            List<Solution> NewSplitSolutions = new List<Solution>(), SolsToBeRemoved = new List<Solution>();
            Solution newsol;
            List<int> SolBoardOutsBuffer = new List<int>();
            IEnumerable<IEnumerable<int>> result;
            int IndexOfSol = -1;
            for(int solindex = 0; solindex < Solutions.Count; solindex++)//foreach (Solution sol in Solutions)
            {
                if (Solutions[solindex].BoardOuts.Contains(0))
                    Solutions[solindex].BoardOuts.RemoveAll(new System.Predicate<int>(PredicateTestZero));

                if (Solutions[solindex].BoardOuts.Count > TargetNumberOfBoardOuts)
                {
                    SolBoardOutsBuffer.Clear();
                    SolBoardOutsBuffer.AddRange(Solutions[solindex].BoardOuts);
                    result = GetCombinations(SolBoardOutsBuffer, TargetNumberOfBoardOuts);
                    SolsToBeRemoved.Add(Solutions[solindex]);

                    foreach (IEnumerable<int> list in result)
                    {
                        newsol = new Solution(); //if combined with another sol, it won't transfer anything other than it's occurences
                        newsol.Occurences = Solutions[solindex].Occurences;
                        foreach (int i in list)
                            newsol.BoardOuts.Add(i);

                        if (CheckIfSolutionExists(newsol.BoardOuts, NewSplitSolutions, out IndexOfSol) == false)
                            NewSplitSolutions.Add(newsol);
                    }

                }
            }

            foreach (Solution sol in SolsToBeRemoved)
                Solutions.Remove(sol);


            foreach (Solution sol in NewSplitSolutions)
            {
                if (CheckIfSolutionExists(sol.BoardOuts, Solutions, out IndexOfSol))
                    Solutions[IndexOfSol].Occurences += sol.Occurences;
                else
                    Solutions.Add(sol);

            }
            //pt er target outs = minimum outs. brug targetouts i denne funktion til at dele sols med mere 
            //end target outs op i stykker som har target out i sig, og add dem derefter til Solutions listen, 
            //enden som nye eller hvis de allerede findes så add deres occurences sammen 
            #endregion

            int highestOccurence = 0, highestOccurenceIndex = -1;
            foreach (Solution sol in Solutions)
            {
                if (sol.Occurences > highestOccurence)
                {
                    if (SolutionIsDifferentFromBoardIns(sol.BoardOuts, BoardIns))
                    {
                        highestOccurence = sol.Occurences;
                        highestOccurenceIndex = Solutions.IndexOf(sol);
                    }
                }
            }

            return Solutions[highestOccurenceIndex];
        }


        private void FindAllSolutionsFromGivenRoot(List<int> OpenConnectors, int TargetNumberOfOuts, bool GotExpanderComp,
            Dictionary<LevelSpecification.ComponentType, int> CompOccurenceMapping, int MinCompRequiredForSol, int MaxCompRequiredForSol, 
            MathComponentModel WhichCompToApply, List<MathComponentModel> Toolbox/*, Solution LastGeneratedSol = null*/,
            List<LevelSpecification.ComponentType> CompsUsedInRoot, int MaxSpecificCompUsable, 
            Dictionary<LevelSpecification.ComponentType, int> FixedComponentsMaxOccurenceMapping, 
            List<LevelSpecification.ComponentType> SolutionMustContain, int MaxOccurencesWanted, bool EnoughOccurenceFound = false)
        {
            if (EnoughOccurenceFound)
                return;

            if (CheckForViability(OpenConnectors, TargetNumberOfOuts, GotExpanderComp, WhichCompToApply, CompOccurenceMapping, MinCompRequiredForSol,
                MaxCompRequiredForSol) == false)
                return;

            //if (LastGeneratedSol != null)
            //    Solutions.Add(LastGeneratedSol);

            if (Toolbox.Count <= 0)
                return;
            
            List<int> NewOpenConnectors = new List<int>();

            int CompOccurence = 0;
            CompOccurenceMapping.TryGetValue(GetTypeFromModel(WhichCompToApply), out CompOccurence);
            CompOccurence++;
            CompOccurenceMapping[GetTypeFromModel(WhichCompToApply)] = CompOccurence;

            if (WhichCompToApply.outLimit > WhichCompToApply.inLimit && CompOccurence > MaxSpecificCompUsable)
                GotExpanderComp = false;
            if (CompOccurence > MaxSpecificCompUsable)
                Toolbox.RemoveAt(GetIndexOfModelType(WhichCompToApply, Toolbox));
            else if (WhichCompToApply.Locked == true)
            {
                if (CompOccurence > FixedComponentsMaxOccurenceMapping[GetTypeFromModel(WhichCompToApply)])
                {
                    Toolbox.RemoveAt(GetIndexOfModelType(WhichCompToApply, Toolbox));
                }
            }

            Solution CurrentSolution = new Solution();
            int[] mathFuncInputs = new int[2];
            List<int> MathFuncOutputs = new List<int>();
            bool SolutionAlreadyExists = false, RequiredTypesFound = true;
            int SolutionIndex = 0, NumberOfComps = 0;
            for (int i = 0; i < OpenConnectors.Count; i++ )
            {
                CurrentSolution = new Solution();
                mathFuncInputs = new int[2];
                MathFuncOutputs = new List<int>();
                NewOpenConnectors = new List<int>();
                CopyListContentToNewList(OpenConnectors, ref NewOpenConnectors);

                if (WhichCompToApply.inLimit == 2 && (i >= NewOpenConnectors.Count-1 || NewOpenConnectors.Count == 1))
                    break;

                mathFuncInputs[0] = OpenConnectors[i];
                if (NewOpenConnectors.Count > 1 && i < OpenConnectors.Count-1)
                    mathFuncInputs[1] = OpenConnectors[i + 1];
                MathFuncOutputs = WhichCompToApply.MathFunc(mathFuncInputs);
                if (MathFuncOutputs[0] != 0)
                    NewOpenConnectors[i] = MathFuncOutputs[0];

                mathFuncInputs.Initialize();


                if (WhichCompToApply.inLimit == 2)
                    NewOpenConnectors.RemoveAt(i + 1);

                if (WhichCompToApply.outLimit == 2)
                    NewOpenConnectors.Insert(i + 1, MathFuncOutputs[1]);


                NewOpenConnectors.RemoveAll(PredicateTestZero);

                MathFuncOutputs.Clear();

                #region create and add solution
                CurrentSolution.CompsNeededForSol.Add(CountCompsUsed(CompOccurenceMapping));
                if (CompsUsedInRoot == null)
                    CompsUsedInRoot = new List<LevelSpecification.ComponentType>();

                CompsUsedInRoot.Add(GetTypeFromModel(WhichCompToApply));      

                NumberOfComps = CountCompsUsed(CompOccurenceMapping);

                if (NumberOfComps >= MinCompRequiredForSol)
                {
                    SolutionAlreadyExists = CheckIfSolutionExists(NewOpenConnectors, Solutions, out SolutionIndex); //not working

                    foreach (LevelSpecification.ComponentType RequiredType in SolutionMustContain)
                    {
                        if (!(CompsUsedInRoot.Contains(RequiredType)))
                            RequiredTypesFound = false;
                    }

                    if (SolutionAlreadyExists && RequiredTypesFound)
                    {
                        Solutions[SolutionIndex].Occurences = Solutions[SolutionIndex].Occurences + 1;
                        if (Solutions[SolutionIndex].Occurences > MaxOccurencesWanted)
                            EnoughOccurenceFound = true;
                        Solutions[SolutionIndex].CompsNeededForSol.Add(NumberOfComps);
                        Solutions[SolutionIndex].WhichComps.Add(CompsUsedInRoot);
                    }
                    else if (RequiredTypesFound)
                    {
                        CopyListContentToNewList(NewOpenConnectors, ref CurrentSolution.BoardOuts);
                        CurrentSolution.Occurences = 1;
                        CurrentSolution.WhichComps.Add(CompsUsedInRoot);
                        CurrentSolution.CompsNeededForSol.Add(NumberOfComps);

                        Solutions.Add(CurrentSolution);
                    }
                }
                RequiredTypesFound = true;
                #endregion

                List<MathComponentModel> NewToolbox;
                Dictionary<LevelSpecification.ComponentType, int> NewCompOccurenceMapping;
                List<LevelSpecification.ComponentType> NewCompsUsedInRoot;
                Dictionary<LevelSpecification.ComponentType, int> NewFixedComponentsMaxOccurenceMapping;
                List<LevelSpecification.ComponentType> NewSolutionMustContain;

                List<int> IndiciesTried = new List<int>();
                int RandomCompIndex;
                while (IndiciesTried.Count < Toolbox.Count)
                {
                    RandomCompIndex = PickRandomNumberBetweenXYExcludingZ(0, Toolbox.Count - 1, ref IndiciesTried);

                    #region makes copies for the next recursion step
                    NewToolbox = new List<MathComponentModel>();
                    foreach (MathComponentModel mcm2 in Toolbox)
                        NewToolbox.Add(mcm2);

                    NewCompOccurenceMapping = new Dictionary<LevelSpecification.ComponentType, int>();
                    foreach (LevelSpecification.ComponentType type in CompOccurenceMapping.Keys)
                        NewCompOccurenceMapping.Add(type, CompOccurenceMapping[type]);

                    NewCompsUsedInRoot = new List<LevelSpecification.ComponentType>();
                    foreach (LevelSpecification.ComponentType type in CompsUsedInRoot)
                        NewCompsUsedInRoot.Add(type);

                    NewFixedComponentsMaxOccurenceMapping = new Dictionary<LevelSpecification.ComponentType, int>();
                    foreach (LevelSpecification.ComponentType type in FixedComponentsMaxOccurenceMapping.Keys)
                        NewFixedComponentsMaxOccurenceMapping.Add(type, FixedComponentsMaxOccurenceMapping[type]);

                    NewSolutionMustContain = new List<LevelSpecification.ComponentType>();
                    foreach (LevelSpecification.ComponentType type in SolutionMustContain)
                        NewSolutionMustContain.Add(type);
                    #endregion

                    FindAllSolutionsFromGivenRoot(NewOpenConnectors, TargetNumberOfOuts, GotExpanderComp, NewCompOccurenceMapping,
                        MinCompRequiredForSol, MaxCompRequiredForSol, Toolbox[RandomCompIndex], NewToolbox, NewCompsUsedInRoot, MaxSpecificCompUsable,
                        NewFixedComponentsMaxOccurenceMapping, NewSolutionMustContain, MaxOccurencesWanted, EnoughOccurenceFound);

                    IndiciesTried.Add(RandomCompIndex);
                }

                #region old foreach
                //foreach (MathComponentModel mcm in Toolbox)
                //{
                //    #region makes copies for the next recursion step
                //    NewToolbox = new List<MathComponentModel>();
                //    foreach (MathComponentModel mcm2 in Toolbox)
                //        NewToolbox.Add(mcm2);

                //    NewCompOccurenceMapping = new Dictionary<LevelSpecification.ComponentType, int>();
                //    foreach (LevelSpecification.ComponentType type in CompOccurenceMapping.Keys)
                //        NewCompOccurenceMapping.Add(type, CompOccurenceMapping[type]);

                //    NewCompsUsedInRoot = new List<LevelSpecification.ComponentType>();
                //    foreach (LevelSpecification.ComponentType type in CompsUsedInRoot)
                //        NewCompsUsedInRoot.Add(type);

                //    NewFixedComponentsMaxOccurenceMapping = new Dictionary<LevelSpecification.ComponentType, int>();
                //    foreach (LevelSpecification.ComponentType type in FixedComponentsMaxOccurenceMapping.Keys)
                //        NewFixedComponentsMaxOccurenceMapping.Add(type, FixedComponentsMaxOccurenceMapping[type]);

                //    NewSolutionMustContain = new List<LevelSpecification.ComponentType>();
                //    foreach (LevelSpecification.ComponentType type in SolutionMustContain)
                //        NewSolutionMustContain.Add(type);
                //    #endregion

                //    FindAllSolutionsFromGivenRoot(NewOpenConnectors, TargetNumberOfOuts, GotExpanderComp, NewCompOccurenceMapping,
                //        MinCompRequiredForSol, MaxCompRequiredForSol, mcm, NewToolbox, NewCompsUsedInRoot, MaxSpecificCompUsable,
                //        NewFixedComponentsMaxOccurenceMapping, NewSolutionMustContain, MaxOccurencesWanted, EnoughOccurenceFound);
                //}
                #endregion
            }
        }


        private List<Solution> Solutions = new List<Solution>();    
        public class Solution
        {
            public List<int> BoardOuts = new List<int>();
            public int Occurences = 0;
            public List<int> CompsNeededForSol = new List<int>();
            public List<List<LevelSpecification.ComponentType>> WhichComps = new List<List<LevelSpecification.ComponentType>>();

            //only used for completely random levels
            public List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>();
            public List<int> BoardIns = new List<int>();

            public string HumanSolution = "";
        }

        #region private helper functions
        private bool CheckForViability(List<int> OpenConnectors, int TargetNumberOfOuts, bool GotExpanderComp, MathComponentModel WhichCompToApply,
            Dictionary<LevelSpecification.ComponentType, int> CompOccurenceMapping, int MinCompRequiredForSol, int MaxCompRequiredForSol)
        {
            //if (OpenConnectors.Count - 1 < TargetNumberOfOuts && !GotExpanderComp && WhichCompToApply.inLimit > WhichCompToApply.outLimit)
            //    return false;

            int CompCount = CountCompsUsed(CompOccurenceMapping);
            if (CompCount > MaxCompRequiredForSol)
                return false;

            return true;
        }

        private int CountCompsUsed(Dictionary<LevelSpecification.ComponentType, int> dict)
        {
            int i = 0, j = 0;
            foreach (LevelSpecification.ComponentType comp in dict.Keys)
            {
                dict.TryGetValue(comp, out j);
                i += j;
            }
            return i;
        }

        private LevelSpecification.ComponentType GetTypeFromModel(MathComponentModel model)
        {
            if (model is PlusComponentModel)
                return LevelSpecification.ComponentType.plus;
            else if (model is MinusComponentModel)
                return LevelSpecification.ComponentType.minus;
            else if (model is DivideComponentModel)
                return LevelSpecification.ComponentType.divide;
            else if (model is MultiplyComponentModel)
                return LevelSpecification.ComponentType.multiply;
            else if (model is ConnectorCrossComponentModel)
                return LevelSpecification.ComponentType.connectorcross;
            else if (model is PlusMinusXComponentModel)
            {
                if (((PlusMinusXComponentModel)model).XX == 1)
                {
                    if (((PlusMinusXComponentModel)model).PlusOrMinus == PlusMinusXComponentModel.PlusFunc)
                        return LevelSpecification.ComponentType.plus1;
                    else
                        return LevelSpecification.ComponentType.minus1;
                }
                else if (((PlusMinusXComponentModel)model).XX == 10)
                {
                    if (((PlusMinusXComponentModel)model).PlusOrMinus == PlusMinusXComponentModel.PlusFunc)
                        return LevelSpecification.ComponentType.plus10;
                    else
                        return LevelSpecification.ComponentType.minus10;
                }
                else
                {
                    if (((PlusMinusXComponentModel)model).PlusOrMinus == PlusMinusXComponentModel.PlusFunc)
                        return LevelSpecification.ComponentType.plus100;
                    else
                        return LevelSpecification.ComponentType.minus100;
                }
            }
            else if (model is SplitterComponentModel)
                return LevelSpecification.ComponentType.splitter;
            else
            {
                if (((FilterComponentModel)model).Filters == 1)
                    return LevelSpecification.ComponentType.filter1;
                else if (((FilterComponentModel)model).Filters == 10)
                    return LevelSpecification.ComponentType.filter10;
                else
                    return LevelSpecification.ComponentType.filter100;
            }
        }
        
        private void CopyListContentToNewList(List<int> List, ref List<int> NewList)
        {
            foreach (int i in List)
                NewList.Add(i);
        }

        private int GetIndexOfModelType(MathComponentModel WhichCompToApply, List<MathComponentModel> Toolbox)
        {
            foreach (MathComponentModel mcm in Toolbox)
            {
                if (mcm is PlusComponentModel && WhichCompToApply is PlusComponentModel)
                    return Toolbox.IndexOf(mcm);
                else if (mcm is MinusComponentModel && WhichCompToApply is MinusComponentModel)
                    return Toolbox.IndexOf(mcm);
                else if (mcm is DivideComponentModel && WhichCompToApply is DivideComponentModel)
                    return Toolbox.IndexOf(mcm);
                else if (mcm is MultiplyComponentModel && WhichCompToApply is MultiplyComponentModel)
                    return Toolbox.IndexOf(mcm);
                else if (mcm is ConnectorCrossComponentModel && WhichCompToApply is ConnectorCrossComponentModel)
                    return Toolbox.IndexOf(mcm);
                else if (mcm is SplitterComponentModel && WhichCompToApply is SplitterComponentModel)
                    return Toolbox.IndexOf(mcm);
                else if (mcm is PlusMinusXComponentModel && WhichCompToApply is PlusMinusXComponentModel)
                {
                    if (((PlusMinusXComponentModel)mcm).XX == ((PlusMinusXComponentModel)WhichCompToApply).XX)
                    {
                        if (((PlusMinusXComponentModel)mcm).PlusOrMinus == ((PlusMinusXComponentModel)WhichCompToApply).PlusOrMinus)
                            return Toolbox.IndexOf(mcm);
                    }
                }
                else if (mcm is FilterComponentModel && WhichCompToApply is FilterComponentModel)
                {
                    if (((FilterComponentModel)mcm).Filters == ((FilterComponentModel)WhichCompToApply).Filters)
                        return Toolbox.IndexOf(mcm);
                }
            }
            return -1;

        }

        private bool CheckIfSolutionExists(List<int> SolutionToTest, List<Solution> SolutionList, out int SolutionIndex)
        {
            bool foundMatch = false;
            List<int> buffer = new List<int>();
            SolutionIndex = -1;
            foreach (Solution sol in SolutionList)
            {
                if (sol.BoardOuts.Count == SolutionToTest.Count)
                {
                    buffer.Clear();
                    buffer.AddRange(SolutionToTest);
                    foreach (int i in sol.BoardOuts)
                    {
                        if (buffer.Contains(i))
                        {
                            buffer.Remove(i);
                            if (buffer.Count == 0)
                                foundMatch = true;
                        }
                        else
                            break;
                    }
                    if (foundMatch)
                    {
                        SolutionIndex = SolutionList.IndexOf(sol);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool SolutionIsDifferentFromBoardIns(List<int> SolList, List<int> BoardIns)
        {
            foreach (int i in SolList)
            {
                if (BoardIns.Contains(i))
                    return false;
            }
            return true;
        }

        private IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetCombinations(list, length - 1)
                .SelectMany(t => list, (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        private bool PredicateTestZero(int i)
        {
            if (i == 0)
                return true;
            else
                return false;
        }

        private int PickRandomNumberBetweenXYExcludingZ(int X, int Y, ref List<int> Z)
        {
            Random RNG = new Random();
            int i = RNG.Next(X, Y + 1);
            //bool shitstorm = true;
            //for(int j = X; j < Y + 2; j++)
            //{
            //    if ((Z.Contains(j)) == false)
            //        shitstorm = false;
            //}
            //if (shitstorm)
            //    throw new Exception("shit");
            while (Z.Contains(i))
                i = RNG.Next(X, Y + 1);
            return i;
        }
        #endregion
    }
}
