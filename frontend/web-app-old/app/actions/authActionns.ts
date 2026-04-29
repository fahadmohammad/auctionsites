'use server';

import { auth } from "@/auth";

export async function getCurrentUser() {
    try {

        const session = await auth();

        if (!session || !session.user) {
            return null;
        }

        return session.user;
    }
    catch (error) {
        console.error("Error fetching user data:", error);
        return null;
    }
}