using System.Collections.Generic;
using System;
using SUHScripts.Functional.UnitType;
using SUHScripts.Functional.Composition;

namespace SUHScripts.Functional
{
    public struct Either<L, R>
    {
        public L Left { get; }
        public R Right { get; }

        public bool IsRight { get; }
        private bool IsLeft => !IsRight;

        internal Either(L left)
        {
            IsRight = false;
            Left = left;
            Right = default(R);
        }

        internal Either(R right)
        {
            IsRight = true;
            Right = right;
            Left = default(L);
        }

        public static implicit operator Either<L, R>(L left) => new Either<L, R>(left);
        public static implicit operator Either<L, R>(R right) => new Either<L, R>(right);

        public static implicit operator Either<L, R>(Left<L> left) => new Either<L, R>(left.Value);
        public static implicit operator Either<L, R>(Right<R> right) => new Either<L, R>(right.Value);

        public TR Match<TR>(Func<L, TR> Left, Func<R, TR> Right)
           => IsLeft ? Left(this.Left) : Right(this.Right);

        public Unit Match(Action<L> Left, Action<R> Right)
           => Match(Left.ToFunc(), Right.ToFunc());

        public IEnumerator<R> AsEnumerable()
        {
            if (IsRight) yield return Right;
        }

        public override string ToString() => Match(l => $"Left({l})", r => $"Right({r})");
    }


}