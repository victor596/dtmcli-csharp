namespace DtmCommon
{
    public class OracleDBSpecial : IDbSpecial
    {
        public string Name => "oracle";

        public string GetInsertIgnoreTemplate(string tableAndValues, string pgConstraint)
            => string.Format("insert into {0}", tableAndValues);

        public string GetPlaceHoldSQL(string sql)
            => sql.Replace('@',':');

        public string GetXaSQL(string command, string xid)
            => throw new DtmException("not support xa now!");
    }

}