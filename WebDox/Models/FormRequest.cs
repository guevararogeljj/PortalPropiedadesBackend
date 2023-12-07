namespace WebDox.Models
{
    public class FormRequest
    {
        public WorkflowRequest workflow_request = new WorkflowRequest();

        public Dictionary<string, object> request_form_attribute = new Dictionary<string, object>();

        public bool synchronous_doc_creation = true;

    }

    public class WorkflowRequest
    {
        public string title { get; set; }
        public string description { get; set; }
        public string request_form_id { get; set; }

    }
}
