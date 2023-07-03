# TODO

- Use ImmutableCollection instead of List
- Add state on story aggregating state on tasks
- Create pagable get-all-stories, get-all-tasks
- Don't call it IFooRepository as that isn't domain speak. IFoos is better
- Don't postfix Event to events as that isn't domain speak
- Consider switching to vertical architecture (https://defrag.github.io/2023/04/04/a-structure-for-the-unstructured.html and https://blog.ploeh.dk/2023/05/29/favour-flat-code-file-folders/)
- Add update and delete handlers  
- Add logging, timing, exception handlers to Application.
- Begin transaction once per request and commit once per request.
- Create console app to initialize database, including migrations (on in web app?)
- Should error messages reflect back value or actual length of string? It can be inferred by logging the command/query and simplifies error reporting
- Return error codes (cases of error DUs) in JSON error response, inspired by https://www.youtube.com/watch?v=AeZC1z8D5xI for Dapr