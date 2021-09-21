export function initialize({ scrollContainer, lastItem, componentInstance }) {
    const observerOptions = {
        root: scrollContainer,
        threshold: 0,
        rootMargin: "0px"
    };

    const observer = new IntersectionObserver(async function (entries) {
        if (entries[0].isIntersecting) {
            await componentInstance.invokeMethodAsync("OnScrollToEnd");
            observer.unobserve(lastItem);
            observer.observe(lastItem);
        }
    },
        observerOptions);

    observer.observe(lastItem);

    return {
        dispose: () => observer.disconnect()
    };
}