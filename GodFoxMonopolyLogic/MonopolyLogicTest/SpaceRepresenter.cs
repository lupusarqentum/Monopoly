using System;
using MonopolyLogic;
using MonopolyLogic.Spaces;
using MonopolyLogic.GameProperties;

namespace MonopolyLogicTest
{
    internal class SpaceRepresenter : ISpaceRepresenter
    {
        private Space representingSpace;

        public void SetSpaceToRepresenting(Space space)
        {
            representingSpace = space;
        }

        internal void LiftMortgage(IMonopolyGame board)
        {
            try
            {
                if (representingSpace is RentaPropertySpace)
                {
                    var spc = representingSpace as RentaPropertySpace;
                    if (spc.RentaProperty is MortgagableRentaProperty)
                        Console.WriteLine((spc.RentaProperty as MortgagableRentaProperty).TryLiftMortgage(board));
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("nre");
            }
        }

        internal void MortgageIt(IMonopolyGame board)
        {
            try
            {
                if (representingSpace is RentaPropertySpace)
                {
                    var spc = representingSpace as RentaPropertySpace;
                    if (spc.RentaProperty is MortgagableRentaProperty)
                        Console.WriteLine((spc.RentaProperty as MortgagableRentaProperty).MortgageIt(board));
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("nre");
            }
        }

        internal void BuildHouse(IMonopolyGame board)
        {
            try
            {
                if (representingSpace is RentaPropertySpace)
                    if ((representingSpace as RentaPropertySpace).RentaProperty is Street)
                        Console.WriteLine(((representingSpace as RentaPropertySpace).RentaProperty as Street).TryBuildHouse(board));
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("nre");
            }
        }

        internal void SellHouse(IMonopolyGame board)
        {
            try
            {
                if (representingSpace is RentaPropertySpace)
                    if ((representingSpace as RentaPropertySpace).RentaProperty is Street)
                        Console.WriteLine(((representingSpace as RentaPropertySpace).RentaProperty as Street).TrySellHouse(board));
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("nre");
            }
        }

        internal RentaProperty GetProperty()
        {
            if (representingSpace is RentaPropertySpace)
                return (representingSpace as RentaPropertySpace).RentaProperty;
            
            return null;
        }

        public override string ToString()
        {
            return representingSpace.GetInfo() + representingSpace.ToString();
        }
    }
}
