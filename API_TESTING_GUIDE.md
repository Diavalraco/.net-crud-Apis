# 🧪 Complete API Testing Guide

## 📋 Testing All 4 CRUD Endpoints

### Base URL
- **HTTP**: `http://localhost:5232`
- **HTTPS**: `https://localhost:7191`

---

## 1️⃣ POST /api/quiz - Create Quiz

### **Purpose**: Create a new quiz question

### **Request Details**:
- **Method**: `POST`
- **URL**: `http://localhost:5232/api/quiz`
- **Headers**: 
  ```
  Content-Type: application/json
  ```
- **Body** (JSON):
```json
{
    "question": "What is the capital of France?",
    "options": [
        "London",
        "Berlin",
        "Paris",
        "Madrid"
    ],
    "correctAnswer": 2,
    "category": "Geography",
    "difficulty": "Easy"
}
```

### **Expected Response**: `201 Created`
```json
{
    "id": "67890abcdef1234567890123",
    "question": "What is the capital of France?",
    "options": [
        "London",
        "Berlin",
        "Paris",
        "Madrid"
    ],
    "correctAnswer": 2,
    "category": "Geography",
    "difficulty": "Easy",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
}
```

### **Important**: 
- **Save the `id` from the response** - you'll need it for GET, PUT, and DELETE
- `correctAnswer` is 0-based index (0 = first option, 1 = second, etc.)

### **Testing in Postman**:
1. Select **POST** method
2. Enter URL: `http://localhost:5232/api/quiz`
3. Go to **Headers** tab, add: `Content-Type: application/json`
4. Go to **Body** tab, select **raw** and **JSON**
5. Paste the JSON body above
6. Click **Send**

---

## 2️⃣ GET /api/quiz/{id} - Get Quiz by ID

### **Purpose**: Retrieve a quiz question by its MongoDB ObjectId

### **Request Details**:
- **Method**: `GET`
- **URL**: `http://localhost:5232/api/quiz/{id}`
  - Replace `{id}` with the actual ID from POST response
  - Example: `http://localhost:5232/api/quiz/67890abcdef1234567890123`
- **Headers**: None required
- **Body**: None

### **Expected Response**: `200 OK`
```json
{
    "id": "67890abcdef1234567890123",
    "question": "What is the capital of France?",
    "options": [
        "London",
        "Berlin",
        "Paris",
        "Madrid"
    ],
    "correctAnswer": 2,
    "category": "Geography",
    "difficulty": "Easy",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
}
```

### **Error Response**: `404 Not Found` (if ID doesn't exist)
```json
{
    "error": "Quiz with ID 67890abcdef1234567890123 not found"
}
```

### **Testing in Postman**:
1. Select **GET** method
2. Enter URL: `http://localhost:5232/api/quiz/PASTE_ID_HERE`
   - Replace `PASTE_ID_HERE` with the ID from POST response
3. Click **Send**

### **Step-by-Step**:
1. First, run **POST /api/quiz** to create a quiz
2. Copy the `id` from the response
3. Use that `id` in the GET request URL

---

## 3️⃣ PUT /api/quiz/{id} - Update Quiz

### **Purpose**: Update an existing quiz question (partial update - only provided fields are updated)

### **Request Details**:
- **Method**: `PUT`
- **URL**: `http://localhost:5232/api/quiz/{id}`
  - Replace `{id}` with the actual ID
- **Headers**: 
  ```
  Content-Type: application/json
  ```
- **Body** (JSON) - **All fields are optional**:

### **Example 1: Update only question and difficulty**
```json
{
    "question": "What is the capital of France? (Updated)",
    "difficulty": "Medium"
}
```

### **Example 2: Update only category**
```json
{
    "category": "World Geography"
}
```

### **Example 3: Update multiple fields**
```json
{
    "question": "What is the capital city of France?",
    "options": [
        "London",
        "Berlin",
        "Paris",
        "Madrid",
        "Rome"
    ],
    "correctAnswer": 2,
    "category": "European Geography",
    "difficulty": "Hard"
}
```

### **Expected Response**: `200 OK`
```json
{
    "id": "67890abcdef1234567890123",
    "question": "What is the capital of France? (Updated)",
    "options": [
        "London",
        "Berlin",
        "Paris",
        "Madrid"
    ],
    "correctAnswer": 2,
    "category": "Geography",
    "difficulty": "Medium",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:35:00Z"
}
```

### **Error Responses**:
- `404 Not Found`: Quiz ID doesn't exist
- `400 Bad Request`: Invalid data (e.g., correctAnswer index out of range)

### **Testing in Postman**:
1. Select **PUT** method
2. Enter URL: `http://localhost:5232/api/quiz/PASTE_ID_HERE`
3. Go to **Headers** tab, add: `Content-Type: application/json`
4. Go to **Body** tab, select **raw** and **JSON**
5. Paste one of the JSON examples above (or create your own)
6. Click **Send**

### **Important Notes**:
- ✅ You can update **any combination** of fields
- ✅ Only fields you provide will be updated
- ✅ Other fields remain unchanged
- ⚠️ If updating `options`, make sure `correctAnswer` index is still valid

---

## 4️⃣ DELETE /api/quiz/{id} - Delete Quiz

### **Purpose**: Delete a quiz question by its ID

### **Request Details**:
- **Method**: `DELETE`
- **URL**: `http://localhost:5232/api/quiz/{id}`
  - Replace `{id}` with the actual ID
- **Headers**: None required
- **Body**: None

### **Expected Response**: `204 No Content`
- No response body
- Status code: 204

### **Error Response**: `404 Not Found` (if ID doesn't exist)
```json
{
    "error": "Quiz with ID 67890abcdef1234567890123 not found"
}
```

### **Testing in Postman**:
1. Select **DELETE** method
2. Enter URL: `http://localhost:5232/api/quiz/PASTE_ID_HERE`
3. Click **Send**
4. Check status code should be `204 No Content`

---

## 🔄 Complete Testing Workflow

### **Recommended Testing Order**:

1. **POST** - Create a quiz
   - Copy the `id` from response
   
2. **GET** - Retrieve the quiz you just created
   - Verify all data matches what you created
   
3. **PUT** - Update the quiz
   - Update some fields
   - Use GET again to verify changes
   
4. **DELETE** - Delete the quiz
   - Verify it's deleted by trying GET again (should return 404)

---

## 📝 Example Test Scenarios

### **Scenario 1: Full CRUD Test**
```bash
# 1. CREATE
POST http://localhost:5232/api/quiz
Body: {
    "question": "What is 2 + 2?",
    "options": ["3", "4", "5", "6"],
    "correctAnswer": 1,
    "category": "Math",
    "difficulty": "Easy"
}
# Response: Get ID = "abc123..."

# 2. READ
GET http://localhost:5232/api/quiz/abc123...
# Response: Should return the quiz

# 3. UPDATE
PUT http://localhost:5232/api/quiz/abc123...
Body: {
    "difficulty": "Medium",
    "question": "What is 2 + 2? (Updated)"
}
# Response: Updated quiz

# 4. DELETE
DELETE http://localhost:5232/api/quiz/abc123...
# Response: 204 No Content

# 5. VERIFY DELETION
GET http://localhost:5232/api/quiz/abc123...
# Response: 404 Not Found (confirms deletion)
```

### **Scenario 2: Error Testing**

**Test Invalid ID:**
```
GET http://localhost:5232/api/quiz/invalid_id_123
# Expected: 404 Not Found
```

**Test Invalid Data:**
```
POST http://localhost:5232/api/quiz
Body: {
    "question": "Test",
    "options": ["A", "B"],
    "correctAnswer": 5,  // Invalid: index out of range
    "category": "Test",
    "difficulty": "Easy"
}
# Expected: 400 Bad Request
```

**Test Missing Required Fields:**
```
POST http://localhost:5232/api/quiz
Body: {
    "question": "Test question"
    // Missing other required fields
}
# Expected: 400 Bad Request with validation errors
```

---

## 🛠️ Using Postman Collection

1. **Import Collection**:
   - Open Postman
   - Click **Import** button
   - Select `Postman_Collection.json` file
   - Collection will be imported with all 4 endpoints

2. **Set Base URL**:
   - Click on collection name
   - Go to **Variables** tab
   - Set `baseUrl` to `http://localhost:5232`

3. **Test Endpoints**:
   - Start with **Create Quiz**
   - Copy the `id` from response
   - Update the `:id` variable in other requests
   - Test GET, PUT, DELETE in sequence

---

## ✅ Response Status Codes Summary

| Status Code | Meaning | When You See It |
|------------|---------|----------------|
| `201 Created` | Successfully created | POST request succeeded |
| `200 OK` | Success | GET or PUT request succeeded |
| `204 No Content` | Successfully deleted | DELETE request succeeded |
| `400 Bad Request` | Invalid input data | Validation failed |
| `404 Not Found` | Resource not found | ID doesn't exist |
| `500 Internal Server Error` | Server error | Something went wrong on server |

---

## 💡 Pro Tips

1. **Always save the ID**: After creating a quiz, save the `id` for testing other endpoints

2. **Test in sequence**: CREATE → READ → UPDATE → DELETE

3. **Verify updates**: After PUT, use GET to confirm changes

4. **Test error cases**: Try invalid IDs, missing fields, invalid data

5. **Use Postman variables**: Set `baseUrl` as a variable for easy switching

6. **Check response headers**: Look for status codes and error messages

---

## 🚀 Quick Test Checklist

- [ ] POST creates a quiz successfully (201)
- [ ] GET retrieves the quiz by ID (200)
- [ ] PUT updates the quiz (200)
- [ ] GET after PUT shows updated data
- [ ] DELETE removes the quiz (204)
- [ ] GET after DELETE returns 404
- [ ] Invalid ID returns 404
- [ ] Invalid data returns 400

---

Happy Testing! 🎉

