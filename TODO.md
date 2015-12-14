#TODO

* [x] Map command line arguments to a method to execute.
* [x] Support boolean switches
* [x] Support implicit ordered parameters
* [x] Support the concept of a default action
* [x] Support sub-comamnds
* [x] Generate help using reflection
* [x] Create a demo project to show how to use the library.
* [x] Support an injectable Logger
* [x] Support customizable conventions
* [x] Handle parsing errors in a friendly manner.
* [x] Provide testability of command structure separate from execution of command structure.
* [ ] Support aliases on controllers
* [ ] Support aliases on methods
* [x] Support aliases on parameters
* [ ] Aliases should be emitted by the help
* [ ] Help should support an output file parameter
* [ ] Help generation should be customizable
* [ ] Support argument parsing for primitive types
  * [x] bool
  * [x] int
  * [ ] long
  * [ ] DateTime
  * [ ] decimal
* [ ] Support argument parsing for nullable primitive types
  * [ ] bool
  * [ ] int
  * [ ] long
  * [ ] DateTime
  * [ ] decimal
* [ ] Support custom argument parsers
* [ ] Support array arguments
* [ ] Support --switch and --no-switch argument styles for boolean arguments
* [ ] Create a nuget package
* [ ] Wire into a teamcity build for publication
* [ ] Validation
  * [ ] Validate no more than one action per command
  * [ ] Validate no more than one of the same alias on a method