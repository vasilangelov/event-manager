function el(name, properties, customProperties) {
    const e = document.createElement(name);

    if (typeof properties === "object") {
        Object.assign(e, properties);
    }

    if (typeof customProperties === 'object') {
        Object.entries(customProperties).forEach(([key, value]) => {
            e.setAttribute(key, value);
        });
    }

    return e;
}

function id(i) {
    return document.getElementById(i);
}
