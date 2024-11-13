using GraphQL;

namespace TOTools.StartGGAPI;

class StartGGQueries
{
    public static GraphQLRequest CreateNumberOfEntrantsQuery(string slug)
    {
        return new GraphQLRequest
        {
            Query = """
                    query GetNumberOfEntrants($slug: String!) {
                        event(slug: $slug) {
                            id
                            numEntrants
                        }
                    }
                    """,
            Variables = new { slug = slug }
        };
    }

    public static GraphQLRequest CreateEntrantsQuery(
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
            Variables = new { slug = slug, numEntrants = numEntrants, page = page }
        };
    }

    public static GraphQLRequest CreateEventSetsQuery(string slug, int page)
    {
        return new GraphQLRequest
        {
            Query = """
                    query GetEventSets($slug: String!, $page: Int!) {
                        event(slug: $slug){
                            sets(perPage: 100, page: $page){
                            	pageInfo{
                            	    totalPages
                                }
                                nodes {
                                    id
                                    identifier
                                    round
                                    slots {
                                        entrant {
                                            id
                                            name
                                        }
                                        prereqId
                                    }
                                    phaseGroup{
                                        phase {
                                            id
                                            bracketType
                                            phaseOrder
                                        }
                                    }
                                }
                            }	
                        }
                    }
                    """,
            Variables = new { slug = slug, page = page }
        };
    }
}