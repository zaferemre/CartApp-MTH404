import axios from 'axios';

private const string ApiUrl = "http://localhost:5012/api/Customer";
; // API URL

// Get all customers
export const getCustomers = async () => {
    try {
        const response = await axios.get(apiUrl);
        return response.data;
    } catch (error) {
        console.error('Error fetching customers:', error.message);
        throw error;
    }
};

// Get a customer by ID
export const getCustomerById = async (id) => {
    try {
        const response = await axios.get(`${apiUrl}/${id}`);
        return response.data;
    } catch (error) {
        console.error(`Error fetching customer with ID ${id}:`, error.message);
        throw error;
    }
};

// Create a new customer
export const createCustomer = async (customer) => {
    try {
        const response = await axios.post(apiUrl, customer);
        return response.data;
    } catch (error) {
        console.error('Error creating customer:', error.message);
        throw error;
    }
};

// Update an existing customer
export const updateCustomer = async (id, customer) => {
    try {
        const response = await axios.put(`${apiUrl}/${id}`, customer);
        return response.data;
    } catch (error) {
        console.error(`Error updating customer with ID ${id}:`, error.message);
        throw error;
    }
};

// Delete a customer
export const deleteCustomer = async (id) => {
    try {
        await axios.delete(`${apiUrl}/${id}`);
    } catch (error) {
        console.error(`Error deleting customer with ID ${id}:`, error.message);
        throw error;
    }
};
