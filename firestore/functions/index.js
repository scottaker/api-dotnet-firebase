/**
 * Import function triggers from their respective submodules:
 *
 * const {onCall} = require("firebase-functions/v2/https");
 * const {onDocumentWritten} = require("firebase-functions/v2/firestore");
 *
 * See a full list of supported triggers at https://firebase.google.com/docs/functions
 */

// const functions = require('firebase-functions');
// const admin = require('firebase-admin');
// admin.initializeApp();


/**
 * Copyright 2022 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// [START imports]
// Dependencies for callable functions.
const { onCall, HttpsError } = require("firebase-functions/v2/https");
const { logger } = require("firebase-functions/v2");
const { getFirestore, Timestamp, FieldValue, Filter } = require('firebase-admin/firestore');
const functions = require('firebase-functions');

// Dependencies for the addMessage function.
const { getDatabase } = require("firebase-admin/database");
const admin = require('firebase-admin');
admin.initializeApp();


// Example Firebase function in JavaScript
exports.collection_count = functions.https.onRequest(async (req, res) => {
    // const default_collection 
    let working_collection = 'complaints';

    const data = req.body;
    try {
        if (data && data.collection) {
            working_collection = data.collection;
        }
        const db = getFirestore();

        const collectionRef = db.collection(working_collection);
        const snapshot = await collectionRef.count().get();
        const count = snapshot.data().count;

        const value = { count: count, collection: working_collection };
        res.status(200).send(value);
    }
    catch (err) {
        res.status(500).send({ error: err, body: data });
    }
});




// const sanitizer = require("./sanitizer");
// [END imports]

/*
exports.call_collection_count = onCall(async (request, response) => {
    const db = getFirestore();
    const collectionRef = db.collection('complaints');
    const snapshot = await collectionRef.count().get();
    const count = snapshot.data().count;

    const value = { count: count };
    return value;
});
*/

// [START v2allAdd]
// [START v2addFunctionTrigger]
// Adds two numbers to each other.
/*
exports.addnumbers = onCall((request) => {
    // [END v2addFunctionTrigger]
    // [START v2readAddData]
    // Numbers passed from the client.
    const firstNumber = request.data.firstNumber;
    const secondNumber = request.data.secondNumber;
    // [END v2readAddData]

    // [START v2addHttpsError]
    // Checking that attributes are present and are numbers.
    if (!Number.isFinite(firstNumber) || !Number.isFinite(secondNumber)) {
        // Throwing an HttpsError so that the client gets the error details.
        throw new HttpsError("invalid-argument", "The function must be called " +
            "with two arguments \"firstNumber\" and \"secondNumber\" which " +
            "must both be numbers.");
    }
    // [END v2addHttpsError]

    // [START v2returnAddData]
    // returning result.
    return {
        firstNumber: firstNumber,
        secondNumber: secondNumber,
        operator: "+",
        operationResult: firstNumber + secondNumber,
    };
    // [END v2returnAddData]
});
*/
// [END v2allAdd]

// [START v2messageFunctionTrigger]
// Saves a message to the Firebase Realtime Database but sanitizes the
// text by removing swearwords.
/*
exports.addmessage = onCall((request, response) => {
    // [START_EXCLUDE]
    // [START v2readMessageData]
    // Message text passed from the client.
    const text = request.data.text;
    // [END v2readMessageData]
    // [START v2messageHttpsErrors]
    // Checking attribute.
    if (!(typeof text === "string") || text.length === 0) {
        // Throwing an HttpsError so that the client gets the error details.
        throw new HttpsError("invalid-argument", "The function must be called " +
            "with one arguments \"text\" containing the message text to add.");
    }
    // Checking that the user is authenticated.
    if (!request.auth) {
        // Throwing an HttpsError so that the client gets the error details.
        throw new HttpsError("failed-precondition", "The function must be " +
            "called while authenticated.");
    }
    // [END v2messageHttpsErrors]

    // [START v2authIntegration]
    // Authentication / user information is automatically added to the request.
    const uid = request.auth.uid;
    const name = request.auth.token.name || null;
    const picture = request.auth.token.picture || null;
    const email = request.auth.token.email || null;
    // [END v2authIntegration]

    // [START v2returnMessageAsync]
    // Saving the new message to the Realtime Database.
    const sanitizedMessage = sanitizer.sanitizeText(text); // Sanitize message.

    return getDatabase().ref("/messages").push({
        text: sanitizedMessage,
        author: { uid, name, picture, email },
    }).then(() => {
        logger.info("New Message written");
        // Returning the sanitized message to the client.
        return { text: sanitizedMessage };
    })
        // [END v2returnMessageAsync]
        .catch((error) => {
            // Re-throwing the error as an HttpsError so that the client gets
            // the error details.
            throw new HttpsError("unknown", error.message, error);
        });
    // [END_EXCLUDE]
});
*/
// [END v2messageFunctionTrigger]


// const { onRequest } = require("firebase-functions/v2/https");
// const logger = require("firebase-functions/logger");

// // Create and deploy your first functions
// // https://firebase.google.com/docs/functions/get-started

// exports.complaint_count = onRequest(async (request, response) => {
//     const collectionRef = db.collection('complaints');
//     const snapshot = await collectionRef.count().get();
//     const count = snapshot.data().count;

//     const body = { count: count };
//     response.status(200).send(body);
// });


// trigger?
/**
exports.aggregateComments = functions.firestore
    .document('posts/{postId}/comments/{commentId}')
    .onWrite(event => {

        const commentId = event.params.commentId;
        const postId = event.params.postId;

        // ref to the parent document
        const docRef = admin.firestore().collection('posts').doc(postId)

        // get all comments and aggregate
        return docRef.collection('comments').orderBy('createdAt', 'desc')
            .get()
            .then(querySnapshot => {

                // get the total comment count
                const commentCount = querySnapshot.size

                const recentComments = []

                // add data from the 5 most recent comments to the array
                querySnapshot.forEach(doc => {
                    recentComments.push(doc.data())
                });

                recentComments.splice(5)

                // record last comment timestamp
                const lastActivity = recentComments[0].createdAt

                // data to update on the document
                const data = { commentCount, recentComments, lastActivity }

                // run update
                return docRef.update(data)
            })
            .catch(err => console.log(err))
    });

     */