using GraphQL;
using GraphQL.Client.Http;

namespace StartGG;

class StartGGQueries
{
    GraphQLRequest CreateNumberOfEntrantsQuery(string slug)
    {
        return new GraphQLRequest
        {
            Query = """
                    query GetNumberOfEntrants($slug: String) {
                        event(slug: $slug) {
                            id
                            numEntrants
                        }
                    }
                    """,
            Variables = new { slug }
        };
    }

    static GraphQLRequest CreateEntrantsQuery(
        string slug,
        int numEntrants,
        int page)
    {
        return new GraphQLRequest
        {
            Query = """
                    query GetEntrants($slug: String!, $numEntrants: Int!, $page: Int!) {
                        event(slug: $slug) {
                            entrants(query: { page: $page, perPage: $numEntrants }) {
                                pageInfo {
                                    perPage
                                    page
                                    totalPages
                                }
                                nodes {
                                    id
                                }
                            }
                        }
                    }
                    """,
            Variables = new { slug, numEntrants, page }
        };
    }
}