import orderApi from 'api/axiosOrder';

export const processOrder = async (order) => {
  try {
    const response = await orderApi.post('/', order);
    return response;
  } catch (error) {
    throw error;
  }
};