function fillInDateTimes() {
    const timeElements = document.querySelectorAll('[data-date-display]');

    for (const el of timeElements) {
        const format = el.dataset.dateCulture ?? "default";
        const dateStyle = el.dataset.dateStyle ?? "short";
        const timeStyle = el.dataset.timeStyle ?? "short";

        el.textContent = new Date(el.dataset.dateDisplay).toLocaleString(format, { dateStyle, timeStyle });
    }
};

document.addEventListener('DOMContentLoaded', fillInDateTimes, { once: true });
