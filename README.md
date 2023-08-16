# WebClient

A web client app that consumes a 3rd party REST API with simple CRUD actions. The code is .NET 7 and I've also tried to keep it as simple as possible.

## Architecture

Inn terms of architecture I'm trying to adhere to Vertical Slice Architecture aka Features Folder.
As you can see in the project structure, I have a folder called Features, and inside it I have a folder for each feature. This allows for easier changes and maintenance of the code-base as well as simpler cohesion and coupling of the code. I've also applied CQRS through MediatR to separate the commands and queries.

### Overview

The Features folders is separate into `Customers` and `ProductWishlist` for the respective resource operations. The top folder files are shared between the two features. This allows for simple code reuse and maintenance as well as easier to understand code.
For a larger code base , I would probably have a separate folder, e.g. `Shared` and a sub-folder for each resource, but for this small project, I think this is enough.

The `DTOs` folder holds any 3rd party DTOs that are used in the project. I could've named it differently but since I am adhering to VSA and the request/responses are inside the feature file itself this seemed like a good option.

The `Data` folder is operations related to data and on this case I'm applying the repository pattern to consume the 3rd party API. I've also applied a delegating handler to the typed HTTP clients to ease the process of reusing ids from the request routes to the destination routes. Might be overkill but seemed a good idea at the time.

The `ConfigOptions` folder is merely to hold the models for accessing app settings. This is more to showcase how I would do it in a larger project/best approach.

## Choices

For the given user stories it seemed logical at first to approach the endpoints in the same manner as the 3rd party API is built. There are other 3rd party endpoints that I did not use as they did not seem, to me, appropriate for what the user stories required. After dogfooding the API for a bit I've enhanced the responses to always return the state of the customer details in order to give a proper representation of the data. It seems more natural given the context.

I've only applied the happy path for the endpoints, the user stories also imply only that and I usually work with additional work items to fulfill other paths. The same can be said with logging. As there is no logging requirement there is no logging in the features.

## Closing thoughts

It seems to fulfill the challenge requirements, the code would be testable, as far as I can tell, following a Chicago approach.
I do feel that something might be improved but given the titles only for the user stories I think this is a good approach.