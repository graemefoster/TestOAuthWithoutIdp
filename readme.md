# WellKnownJwtTestyer

## TLDR;

Ever written an Asp.Net MVC test using WebApplicationFactory, but disabled Authentication and Authorisation as it's too hard to setup?

This project shows how with a bit of configuration and a sneaky addition to the Asp.Net Authentication middleware, you can test your API using all sorts of valid and invalid JWT tokens, without touching your application code.

## Inner Workings

OAuth2 flows work thanks to Public / Private key pairs. You ask for a token from an IdP that you can use to access an API. It authenticates you, and if all is well, mints you a token. It signs the token.
That signature is the important bit. You pass the token to the API, and it validates the signature. This process involves the API going to the IdP and asking for Public Keys that can validate the signature produced with the private key.

OAuth2 wraps the API side up using the discovery endpoint...
So what-if you took over the discovery endpoint for a test application and provided your own signing keys?

That's how this sample works.

The test code adds a ```HttpMessageHandler``` into the Http Pipeline that the ```Microsoft.Identity.Web``` library uses. The custom middleware intercepts 2 calls... The first is for the discovery metadata. The 2nd is for the Public Keys. 

This allows the test code to mint tokens that look and feel like the real tokens issued by your IdP, but are signed and produced in-memory.



