namespace OhpenExt.info
{
    public class Result
    {
        public bool Success { get; set; }

        public string? Message { get; set; }


        public static implicit operator bool(Result _result)
        {
            return _result.Success;
        }
    }
}
