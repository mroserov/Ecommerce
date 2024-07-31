import { v4 as uuidv4 } from 'uuid';

export const getUserId = (userEmail = null) => {
  // user is login use email
  if (userEmail) {
    return userEmail;
  }

  // get of local storage
  let userId = localStorage.getItem('userId');
  if (!userId) {
    // create UUID for new user
    userId = uuidv4();
    localStorage.setItem('userId', userId);
  }
  
  return userId;
};
