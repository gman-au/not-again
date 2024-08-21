# not-again
<p align="center">
<img style="border-radius:10px;" width="400" src="https://github.com/user-attachments/assets/adc7e768-44fe-4965-8884-0f1ac9447bb0" />
</p>

[![test](https://github.com/gman-au/not-again/actions/workflows/test.yml/badge.svg)](https://github.com/gman-au/not-again/actions/workflows/test.yml)

![GitHub Release](https://img.shields.io/github/v/release/gman-au/not-again)

![Docker Version](https://img.shields.io/docker/v/gman82/not-again-api)

[Docker image](https://hub.docker.com/repository/docker/gman82/not-again-api)

## Summary
This is a very simple, lightweight API that can be set up to persist test runs in an (SQL Server) database.

Although written with plugins for C#, the API itself is agnostic and can accept check and result submissions from any language / technology.

When integrated with your test project, the API will be queried before each test is run to determine whether or not it _should_ be run, based on the criteria you have defined.

In CI/CD pipelines and / or large-scale automated test suites, this tool can be useful in minimising continual 're-running' of long tests that are deemed fairly reliable.

For example, you may have several long-running tests that block out the rest of your CI/CD test pipeline, or length the overall testing step(s) by an order of magnitude.

Using the Not-Again API, you could conditionally run the test(s) such that:
* If a test has run in the past X days, and passed, then do not re-run it
* If it has been modified since the last run, then re-run it, regardless
* Otherwise just re-run it

### Examples
#### Example - first test run
```mermaid
sequenceDiagram
    My Test->>+Not-Again: Run this test? Threshold is 10 days
    Not-Again->>+Database: Find last test run
    Database-->>-Not-Again: No test run found, must be a new test
    Not-Again->>-My Test: Yes
    My Test->>+Not-Again: Test completed with this result
    Not-Again->>-Database: Store this result    
```
#### Example - subsequent (fresh) test result run
```mermaid
sequenceDiagram
    My Test->>+Not-Again: Run this test? Threshold is 10 days
    Not-Again->>+Database: Find last test run
    Database-->>-Not-Again: Test run found from 4 days ago, unmodified
    Not-Again->>-My Test: No
```
#### Example - subsequent (stale) test result run
```mermaid
sequenceDiagram
    My Test->>+Not-Again: Run this test? Threshold is 10 days
    Not-Again->>+Database: Find last test run
    Database-->>-Not-Again: Test run found from 25 days ago, unmodified
    Not-Again->>-My Test: Yes
    My Test->>+Not-Again: Test completed with this result
    Not-Again->>-Database: Store this result      
```
#### Example - test result run that has been modified
```mermaid
sequenceDiagram
    My Test->>+Not-Again: Run this test? Threshold is 10 days
    Not-Again->>+Database: Find last test run
    Database-->>-Not-Again: Test run found from 2 days ago, modified since
    Not-Again->>-My Test: Yes
    My Test->>+Not-Again: Test completed with this result
    Not-Again->>-Database: Store this result      
```