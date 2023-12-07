using DataSource.Entities;
using LinqKit;

namespace DataSource.Expressions
{
    public class PropertiesExpression
    {
        private ExpressionStarter<TPROPERTIES> predicate;

        public PropertiesExpression()
        {
            this.predicate = PredicateBuilder.New<TPROPERTIES>();
        }

        public void AddPropertyType(int? value)
        {
            this.predicate.And(x => x.IDTYPENavigation.ID == value); // todo: change for int value
        }

        public void AddState(int? value)
        {
            this.predicate.And(x => x.TADDRESSES.IDCITYv2Navigation.CODESTATENavigation.ID == value);
        }

        public void AddCity(int? value)
        {
            this.predicate.And(x => x.TADDRESSES.IDCITYv2 == value);
        }

        public void AddRooms(int? rooms)
        {
            this.predicate.And(x => x.IDBATHROOMNavigation.DESCRIPTION.Equals(rooms));
        }

        public void AddBathrooms(int? bathrooms)
        {
            this.predicate.And(x => x.IDBATHROOMNavigation.DESCRIPTION.Equals(bathrooms));
        }

        public void AddProceduralStage(int? stage)
        {
            this.predicate.And(x => x.IDPROCEDURALSTAGE == stage);
        }

        public void AddPrice(decimal? price)
        {
            this.predicate.And(x => x.SALEPRICE >= price);
        }

        public void AddStatus(int? status)
        {
            this.predicate.And(x => x.IDSTATUS == status);
        }

        public ExpressionStarter<TPROPERTIES> Expression()
        {
            return this.predicate;
        }






    }
}
