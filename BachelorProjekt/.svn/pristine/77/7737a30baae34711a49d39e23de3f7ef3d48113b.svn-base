using System;
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
using Game2;

namespace Game2
{
    public class LevelCollection
    {
        #region tutorial levels
        public static ResultCollection TutorialSpec1()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>() {  };
            List<int> BoardIns = new List<int>() { 6 };
            List<int> BoardOuts = new List<int>() { 6 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec2()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()
            {
                LevelSpecification.ComponentType.plus
            };
            List<int> BoardIns = new List<int>() { 3, 6 };
            List<int> BoardOuts = new List<int>() { 9 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec3()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>()
            {
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.minus, 6, 5)
            };
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>();
            List<int> BoardIns = new List<int>() { 5, 4 };
            List<int> BoardOuts = new List<int>() { 1 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec4()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 2, 4 };
            List<int> BoardOuts = new List<int>() { 4, 2 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec5()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.filter10
            };
            List<int> BoardIns = new List<int>() { 23 };
            List<int> BoardOuts = new List<int>() { 20, 3 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec6()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>()            
            {
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.plus, 3, 5)
            };
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus
            };
            List<int> BoardIns = new List<int>() { 5, 5, 3 };
            List<int> BoardOuts = new List<int>() { 7 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec7()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.multiply
            };
            List<int> BoardIns = new List<int>() { 10 };
            List<int> BoardOuts = new List<int>() { 100 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec8()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.plus
            };
            List<int> BoardIns = new List<int>() { 100, 5 };
            List<int> BoardOuts = new List<int>() { 50, 55 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec9()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.filter10
            };
            List<int> BoardIns = new List<int>() { 37 };
            List<int> BoardOuts = new List<int>() { 15, 7 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TutorialSpec10()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.filter1,
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.filter100,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 123 };
            List<int> BoardOuts = new List<int>() { 100, 20, 3 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }

        #endregion
        #region left arm levels
        public static ResultCollection LeftArmSpec1()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 8, 10, 14 };
            List<int> BoardOuts = new List<int>() { 12 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec2()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 25, 10, 4 };
            List<int> BoardOuts = new List<int>() { 11 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec3()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>()
            {
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.splitter, 3, 4)
            };
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 12, 4, 7 };
            List<int> BoardOuts = new List<int>() { 10 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec4()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 12, 4, 7 };
            List<int> BoardOuts = new List<int>() { 10 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec5()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.plus1,
                LevelSpecification.ComponentType.plus10,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 17, 6, 11 };
            List<int> BoardOuts = new List<int>() { 3 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec6()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.minus1,
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 33, 108, 3 };
            List<int> BoardOuts = new List<int>() { 67, 10 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec7()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus1,
                LevelSpecification.ComponentType.plus10,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 17, 6, 11 };
            List<int> BoardOuts = new List<int>() { 3 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec8()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.minus10,
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.filter1,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 942, 5, 46 };
            List<int> BoardOuts = new List<int>() { 902, 21 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec9()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.plus10,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 1, 2, 3, 4 };
            List<int> BoardOuts = new List<int>() { 45 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec10()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.minus1,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 9, 6, 72 };
            List<int> BoardOuts = new List<int>() { 69, 5 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec11()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.plus1,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 76, 3, 9, 5 };
            List<int> BoardOuts = new List<int>() { 67 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection LeftArmSpec12()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>()
            {
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.multiply, 2, 8)
            };
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.plus1,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 77, 4, 9, 5 };
            List<int> BoardOuts = new List<int>() { 220, 67 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        #endregion
        #region right arm levels
        public static ResultCollection RightArmSpec1()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.filter100,
                LevelSpecification.ComponentType.plus100
            };
            List<int> BoardIns = new List<int>() { 7, 13, 2 };
            List<int> BoardOuts = new List<int>() { 22 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection RightArmSpec2()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>()
            {
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.filter1, 3, 3)
            };
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 123, 7, 8 };
            List<int> BoardOuts = new List<int>() { 18 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection RightArmSpec3()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.filter1,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 5, 15, 3, 12 };
            List<int> BoardOuts = new List<int>() { 25 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection RightArmSpec4()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>()
            {
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.connectorcross, 6, 8)
            };
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.filter1
            };
            List<int> BoardIns = new List<int>() { 5, 8, 24 };
            List<int> BoardOuts = new List<int>() { 4, 33 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection RightArmSpec5()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 14, 19, 26, 54 };
            List<int> BoardOuts = new List<int>() { 8, 13 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        #endregion
        #region torso levels
        public static ResultCollection TorsoSpec1()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 4, 13, 1 };
            List<int> BoardOuts = new List<int>() { 16 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TorsoSpec2()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 21, 27, 6 };
            List<int> BoardOuts = new List<int>() { 70 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TorsoSpec3()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 21, 27, 6, 71 };
            List<int> BoardOuts = new List<int>() { 70 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TorsoSpec4()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 8, 34 };
            List<int> BoardOuts = new List<int>() { 70 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TorsoSpec5()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.minus10,
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 17, 21, 2 };
            List<int> BoardOuts = new List<int>() { 48 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TorsoSpec6()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.plus1,
                LevelSpecification.ComponentType.minus1,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 19, 60, 26, 13, 73 };
            List<int> BoardOuts = new List<int>() { 20, 23, 103 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TorsoSpec7()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>()
            {
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.minus1, 3, 3),
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.minus1, 3, 5),
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.minus1, 3, 7)
            };
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 8, 34, 54 };
            List<int> BoardOuts = new List<int>() { 70 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TorsoSpec8()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.filter1,
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 213, 11, 27 };
            List<int> BoardOuts = new List<int>() { 105, 14, 78 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection TorsoSpec9()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>()
            {
                new LevelSpecification.FixedComponent(LevelSpecification.ComponentType.filter100, 8, 6)
            };
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.plus,
                LevelSpecification.ComponentType.plus1,
                LevelSpecification.ComponentType.multiply,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 29, 11, 63, 30 };
            List<int> BoardOuts = new List<int>() { 40, 100, 78};

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        #endregion
        #region head levels
        public static ResultCollection HeadSpec1()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.filter1,
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 56, 111 };
            List<int> BoardOuts = new List<int>() { 55 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        public static ResultCollection HeadSpec2()
        {
            List<LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
            List<LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>()            
            {
                LevelSpecification.ComponentType.minus,
                LevelSpecification.ComponentType.splitter,
                LevelSpecification.ComponentType.filter1,
                LevelSpecification.ComponentType.filter10,
                LevelSpecification.ComponentType.connectorcross
            };
            List<int> BoardIns = new List<int>() { 22, 56, 111 };
            List<int> BoardOuts = new List<int>() { 66, 90 };

            return new ResultCollection(FixedComps, Toolbox, BoardIns, BoardOuts);
        }
        #endregion

        public static List<Func<ResultCollection>> LevelCollectionTutorialList = new List<Func<ResultCollection>>()
        {
            TutorialSpec1,
            TutorialSpec2,
            TutorialSpec3,
            TutorialSpec4,
            TutorialSpec5,
            TutorialSpec6,
            TutorialSpec7,
            TutorialSpec8,
            TutorialSpec9,
            TutorialSpec10
        };
        public static List<Func<ResultCollection>> LevelCollectionHeadList = new List<Func<ResultCollection>>()
        {
            HeadSpec1,
            HeadSpec2
        };
        public static List<Func<ResultCollection>> LevelCollectionTorsoList = new List<Func<ResultCollection>>()
        {
            TorsoSpec1,
            TorsoSpec2,
            TorsoSpec3,
            TorsoSpec4,
            TorsoSpec5,
            TorsoSpec6,
            TorsoSpec7,
            TorsoSpec8,
            TorsoSpec9
        };
        public static List<Func<ResultCollection>> LevelCollectionRightArmList = new List<Func<ResultCollection>>()
        {
            RightArmSpec1,
            RightArmSpec2,
            RightArmSpec3,
            RightArmSpec4,
            RightArmSpec5
        };
        public static List<Func<ResultCollection>> LevelCollectionLeftArmList = new List<Func<ResultCollection>>()
        {
            LeftArmSpec1,
            LeftArmSpec2,
            LeftArmSpec3,
            LeftArmSpec4,
            LeftArmSpec5,
            LeftArmSpec6,
            LeftArmSpec7,
            LeftArmSpec8,
            LeftArmSpec9,
            LeftArmSpec10,
            LeftArmSpec11,
            LeftArmSpec12
        };

        public static List<Func<ResultCollection>> GetListFromSection(LevelSpecification.SectionTypes type)
        {
            List<Func<ResultCollection>> list;
            switch(type)
            {
                case LevelSpecification.SectionTypes.Head:
                    list = LevelCollectionHeadList;
                    break;
                case LevelSpecification.SectionTypes.Torso:
                    list = LevelCollectionTorsoList;
                    break;
                case LevelSpecification.SectionTypes.LeftArm:
                    list = LevelCollectionLeftArmList;
                    break;
                case LevelSpecification.SectionTypes.RightArm:
                    list = LevelCollectionRightArmList;
                    break;
                case LevelSpecification.SectionTypes.Custom:
                default:
                    list = LevelCollectionTutorialList;
                    break;
            }
            return list;
        }
    }

    public class ResultCollection
    {
        public ResultCollection(List<Game2.LevelSpecification.FixedComponent> fixedComps, List<Game2.LevelSpecification.ComponentType> toolbox, List<int> boardIns, List<int> boardOuts)
        {
            FixedComps = fixedComps;
            Toolbox = toolbox;
            BoardIns = boardIns;
            BoardOuts = boardOuts;
        }
        public List<Game2.LevelSpecification.FixedComponent> FixedComps = new List<LevelSpecification.FixedComponent>();
        public List<Game2.LevelSpecification.ComponentType> Toolbox = new List<LevelSpecification.ComponentType>();
        public List<int> BoardIns = new List<int>();
        public List<int> BoardOuts = new List<int>();
    }
}
