export const fetchData = async (url, method, data = null) => {
    try {
        const response = await fetch(url, {
            method: method,
            headers: {
                'Content-Type': 'application/json'
            },
            body: data ? JSON.stringify(data) : null
        })

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
            return;
        }
        
        return await response.json()
    }
    catch (e) {
        toastr.error(e);
    }
}

export const isPasswordValid = (password) => {
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$/;
    if (!passwordRegex.test(password)) {
        return false;
    }
    return true;
}