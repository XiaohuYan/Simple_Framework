using System.Collections.Generic;
using System.Reflection;

namespace SimpleFramework.StateMachine.HSM
{
    public class StateMachineBuilder
    {
        private readonly State root;

        public StateMachineBuilder(State root)
        {
            this.root = root;
        }

        public HSMStateMachine Build()
        {
            var m = new HSMStateMachine(root);
            Wire(root, m, new HashSet<State>());
            return m;
        }

        void Wire(State s, HSMStateMachine m, HashSet<State> visited)
        {
            if (s == null)
            {
                return;
            }

            // 访问过了则不需要再初始化了
            if (!visited.Add(s))
            {
                return;
            }

            // 
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

            var machineField = typeof(State).GetField("Machine", flags);

            if (machineField != null) machineField.SetValue(s, m);

            foreach (var fld in s.GetType().GetFields(flags))
            {
                if (!typeof(State).IsAssignableFrom(fld.FieldType))
                {
                    // Only consider fields that are State
                    continue;
                }
                if (fld.Name == "Parent")
                {
                    continue; // Skip back-edge to parent
                }

                var child = (State)fld.GetValue(s);
                if (child == null)
                {
                    continue;
                }
                if (!ReferenceEquals(child.Parent, s))
                {
                    continue; // Ensure it's actually our direct child
                }

                Wire(child, m, visited); // Recurse into the child
            }
        }

    }
}