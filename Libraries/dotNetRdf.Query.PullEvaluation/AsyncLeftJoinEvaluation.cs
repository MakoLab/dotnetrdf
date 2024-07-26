using VDS.RDF;
using VDS.RDF.Nodes;
using VDS.RDF.Query;
using VDS.RDF.Query.Algebra;
using VDS.RDF.Query.Filters;

namespace dotNetRdf.Query.PullEvaluation;

internal class AsyncLeftJoinEvaluation(IAsyncEvaluation lhs, IAsyncEvaluation rhs, string[] joinVars, string[] rhsVars, ISparqlFilter? filter)
    : AbstractAsyncJoinEvaluation(lhs, rhs)
{
    private readonly LinkedList<LhsSolution> _leftSolutions = new();
    private readonly LinkedList<ISet> _rightSolutions = new();
    
    protected override  IEnumerable<ISet> ProcessLhs(PullEvaluationContext context, ISet lhSolutionSet, IRefNode? activeGraph)
    {
        var lhSolution = new LhsSolution(lhSolutionSet);
        if (_rhsHasMore)
        {
            _leftSolutions.AddLast(lhSolution);
            return _rightSolutions.Where((rhSolution) => lhSolution.IsCompatibleWith(rhSolution, joinVars))
                .Select(rhSolution => lhSolution.FilterJoin(rhSolution, s => Filter(s, context, activeGraph)))
                .WhereNotNull();
        }

        var joinSolutions = _rightSolutions
            .Where(rhSolution => lhSolution.IsCompatibleWith(rhSolution, joinVars))
            .Select(rhSolution => lhSolution.FilterJoin(rhSolution, s=>Filter(s, context, activeGraph)))
            .WhereNotNull()
            .ToList();
        return joinSolutions.Count == 0 ? lhSolutionSet.AsEnumerable() : joinSolutions;
    }

    protected override IEnumerable<ISet> ProcessRhs(PullEvaluationContext context, ISet rhSolution, IRefNode? activeGraph)
    {
        if (_lhsHasMore)
        {
            _rightSolutions.AddLast(rhSolution);
        }

        return _leftSolutions.Where(lhSolution => lhSolution.IsCompatibleWith(rhSolution, joinVars))
            .Select(lhSolution => lhSolution.FilterJoin(rhSolution, s => Filter(s, context, activeGraph)))
            .WhereNotNull();
    }

    protected override IEnumerable<ISet>? OnLhsDone(PullEvaluationContext context)
    {
        _rightSolutions.Clear();
        return null;
    }

    protected override IEnumerable<ISet> OnRhsDone(PullEvaluationContext context)
    {
        ISet emptyRhs = new Set();
        foreach (var variable in rhsVars)
        {
            emptyRhs.Add(variable, null);
        }
        IList<ISet> addResults = 
            _leftSolutions.Where(s => !s.Joined)
                .Select(s=>s.Join(emptyRhs))
                .ToList();
        _leftSolutions.Clear();
        return addResults;
    }

    private bool Filter(ISet s, PullEvaluationContext context, IRefNode? activeGraph)
    {
        if (filter == null) return true;
        try
        {
            return filter.Expression.Accept(context.ExpressionProcessor, context, new ExpressionContext(s, activeGraph)).AsSafeBoolean();
        }
        catch (RdfQueryException)
        {
            return false;
        }
    }

    private class LhsSolution(ISet set) : ISet
    {
        public bool Joined { get; private set; }

        public bool Equals(ISet? other)
        {
            return set.Equals(other);
        }

        public void Add(string variable, INode value)
        {
            set.Add(variable, value);
        }

        public bool ContainsVariable(string variable)
        {
            return set.ContainsVariable(variable);
        }

        public bool IsCompatibleWith(ISet s, IEnumerable<string> vars)
        {
            return set.IsCompatibleWith(s, vars);
        }

        public bool IsMinusCompatibleWith(ISet s, IEnumerable<string> vars)
        {
            return set.IsMinusCompatibleWith(s, vars);
        }

        public int ID
        {
            get => set.ID;
            set => set.ID = value;
        }

        public void Remove(string variable)
        {
            set.Remove(variable);
        }

        public INode this[string variable] => set[variable];

        public IEnumerable<INode> Values => set.Values;

        public IEnumerable<string> Variables => set.Variables;

        public ISet Join(ISet other)
        {
            return set.Join(other);
        }

        public ISet? FilterJoin(ISet other, Func<ISet, bool>? filterFunc = null)
        {
            ISet joinResult = set.Join(other);
            if (filterFunc != null)
            {
                if (filterFunc(joinResult) == false)
                {
                    return null;
                }
            }
            Joined = true;
            return set.Join(other);
        }

        public ISet Copy()
        {
            return set.Copy();
        }

        public bool BindsAll(IEnumerable<string> vars)
        {
            return set.BindsAll(vars);
        }
    }
}