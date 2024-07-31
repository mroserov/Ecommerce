import catalogApi from "api/axiosCatalog";

export const getProductById = async (id) => {
    try {
        const response =await catalogApi.post('/graphql', {
          query: `
            query {
              productById(id: ${id}) {
                id
                stock
              }
            }
          `,
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching Product by is', error);
        throw error;
    }
};

export const getProducts = async (searchTerm = '', page, pageSize) => {
    try {
        const response = await catalogApi.post('/', {
          query: `
            query ($searchTerm: String, $page: Int!, $pageSize: Int! ) {
              products(searchTerm: $searchTerm, pageNumber: $page, pageSize: $pageSize) {
                currentPage
                pageSize
                totalCount
                totalPages
                items {
                  createdAt
                  description
                  discount
                  id
                  imageUrl
                  name
                  price
                  slug
                  stock
                  updatedAt
                }
              }
            }
          `,
          variables: {
            page,
            pageSize,
            searchTerm,
          },
        });
        return response.data;
    } catch (error) {
        console.error('Error fetching Products', error);
        throw error;
    }
};