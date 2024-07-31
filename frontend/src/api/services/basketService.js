import basketApi from "api/axiosBasket";

// get cart by user
export const getShoppingCart = async (userId) => {
    try {
        const response = await basketApi.get(`/${userId}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching shopping cart', error);
        throw error;
    }
};

// add product
export const addItemToShoppingCart = async (userId, item) => {
    try {
        const response = await basketApi.post(`/${userId}/items`, item);
        return response.data;
    } catch (error) {
        console.error('Error adding item to shopping cart', error);
        throw error;
    }
};

// update product
export const updateItemInShoppingCart = async (userId, item) => {
    try {
        const response = await basketApi.put(`/${userId}/items`, item);
        return response.data;
    } catch (error) {
        console.error('Error updating item in shopping cart', error);
        throw error;
    }
};

// delete product
export const removeItemFromShoppingCart = async (userId, productId) => {
    try {
        const response = await basketApi.delete(`/${userId}/items/${productId}`);
        return response.data;
    } catch (error) {
        console.error('Error removing item from shopping cart', error);
        throw error;
    }
};

// delete cart
export const clearShoppingCart = async (userId) => {
    try {
        const response = await basketApi.delete(`/${userId}`);
        return response.data;
    } catch (error) {
        console.error('Error clearing shopping cart', error);
        throw error;
    }
};
